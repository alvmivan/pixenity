using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class ExplorerView : MonoBehaviour
    {
        public Button goUpBUtton;

        public TextMeshProUGUI currentDirectoryLabel;
        public TextMeshProUGUI currentFileLabel;
        public TextMeshProUGUI currentFileContent;
        
        public DataButton directoryButtonPrefab;
        public DataButton fileButtonPrefab;

        public RectTransform directoriesContent;
        public RectTransform filesContent;
        
        
        const string LastDirectoryStoredKey = "Bones.FileExplorer.LastDirectoryStored";
        LoadSaveFiles explorer = new LoadSaveFiles();
    
        public string extension = ".meta";

        private DirectoryInfo LastDirectoryStored()
        {
            var stored = PlayerPrefs.GetString(LastDirectoryStoredKey);
            if (string.IsNullOrEmpty(stored) || !explorer.ValidateDirectory(stored, out var dir))
            {
                return explorer.Root();
            }
            return dir;
        }

        private DirectoryInfo current;
        private void Start()
        {
            DisplayDirectory(LastDirectoryStored());
            goUpBUtton.onClick.AddListener(() =>
            {
                DisplayDirectory(explorer.GetParentDirectory(current));
            });
        }

        private void DisplayDirectory(DirectoryInfo directory)
        {
            current = directory;
            StoreDirectory(directory);

            

            ClearChildren(filesContent.gameObject);
            var files = explorer.GetFiles(directory, extension);
            foreach (var fileInfo in files)
            {
                var fileButton  = Instantiate(fileButtonPrefab, filesContent);
                fileButton["file"] = fileInfo.FilePath;
                fileButton.GetComponentInChildren<TextMeshProUGUI>().text = explorer.GetFileName(fileInfo);
                fileButton.Button.onClick.AddListener(() =>
                {
                    OpenFile(fileInfo);
                });
            }
            
            

            ClearChildren(directoriesContent.gameObject);
            var directories = explorer.GetChildrenDirectories(directory);
            foreach (var dirInfo in directories)
            {
                var directoryButton  = Instantiate(directoryButtonPrefab, directoriesContent);
                directoryButton.GetComponentInChildren<TextMeshProUGUI>().text = explorer.GetDirectoryName(dirInfo);
                directoryButton["directory"] = dirInfo.DirectoryPath;
                directoryButton.Button.onClick.AddListener(() =>
                {
                    DisplayDirectory(dirInfo);
                });
            }

            currentDirectoryLabel.text = directory.DirectoryPath;
        }

        private void OpenFile(FileInfo fileInfo)
        {
            var content = explorer.Load(fileInfo);
            currentFileContent.text = content;
            currentFileLabel.text = explorer.GetFileName(fileInfo);
        }

        private void ClearChildren(GameObject go)
        {
            var childCount = go.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                childrenQueue.Enqueue(go.transform.GetChild(i));
            }

            while (childrenQueue.Count > 0)
            {
                Destroy(childrenQueue.Dequeue().gameObject);
            }
        }
        
        Queue<Transform> childrenQueue = new Queue<Transform>();

        private void StoreDirectory(DirectoryInfo directory)
        {
            PlayerPrefs.SetString(LastDirectoryStoredKey, directory.DirectoryPath);
            PlayerPrefs.Save();
        }
    }
}