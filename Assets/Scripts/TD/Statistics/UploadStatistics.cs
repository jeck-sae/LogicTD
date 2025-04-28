using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGoogleDrive;

namespace TowerDefense
{
    [RequireComponent(typeof(StatisticsTracker))]
    public class UploadStatistics : MonoBehaviour
    {
        [Serializable]
        public class StatHistory
        {
            public List<StatisticsTracker.GameStats> stats = new List<StatisticsTracker.GameStats>();
        }
        
        public string localFileName = "statistics.json";
        public StatHistory history = new StatHistory();
        
        private void Start()
        {
            GameEvents.OnGameOver += SaveStats;
            StartCoroutine(LoadStatsCoroutine());
        }

        private void OnDestroy()
        {
            GameEvents.OnGameOver -= SaveStats;
        }

        private void SaveStats()
        {
            StartCoroutine(SaveStatsCoroutine());
        }   
        private IEnumerator SaveStatsCoroutine()
        {
            var s = GetComponent<StatisticsTracker>();
            history.stats.Add(s.stats);
            
            string id = SystemInfo.deviceUniqueIdentifier;
            
            var json = JsonUtility.ToJson(history);
            
            //local save
            var path = Path.Combine(Application.persistentDataPath, localFileName);
            File.WriteAllText(path, json);
            
            Debug.Log(path);
            
            //cloud save
            var bytes = Encoding.UTF8.GetBytes(json);
            var file = new UnityGoogleDrive.Data.File() { Name = $"stats-{id}.json", Content = bytes };
            
            //create cloud file
            if (string.IsNullOrEmpty(SaveManager.Instance.data.statisticsFileId))
            {
                var request = GoogleDriveFiles.Create(file);
                yield return request.Send();
                SaveManager.Instance.data.statisticsFileId = request.ResponseData.Id;
                SaveManager.SaveState();
            }
            else //update cloud file
            {
                var request = GoogleDriveFiles.Update(SaveManager.Instance.data.statisticsFileId, file);
                yield return request.Send();
            }
        }

        private IEnumerator LoadStatsCoroutine()
        {
            //get local save
            var path = Path.Combine(Application.persistentDataPath, localFileName);
            if (File.Exists(path))
                history = JsonUtility.FromJson<StatHistory>(File.ReadAllText(path));
            
            //ignore cloud save if there is none
            if (string.IsNullOrEmpty(SaveManager.Instance.data.statisticsFileId))
            {
                history = new StatHistory();
                yield break;
            }
            
            //get cloud save
            var request = GoogleDriveFiles.Download(SaveManager.Instance.data.statisticsFileId);
            yield return request.Send();
            var content = request.ResponseData.Content;
            
            if (content != null && content.Length > 0)
            {
                var text = Encoding.ASCII.GetString(content);
                var cloudHistory = JsonUtility.FromJson<StatHistory>(text);
                
                //load the version with the most games
                if (cloudHistory.stats.Count > history.stats.Count)
                    history = cloudHistory;
            }
        }
    }
}
