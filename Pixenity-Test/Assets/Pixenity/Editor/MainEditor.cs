using UnityEditor;
using UnityEngine;

namespace Pixenity.Editor
{
    public class MainEditor : EditorWindow, MainScreen
    {

        private MainScreenPresenter presenter;
        
        [MenuItem("Torque Games/Pixenity")]
        private static void ShowWindow()
        {
            
            MainEditor window = GetWindow<MainEditor>();
            window.presenter = new MainScreenPresenter(window, new EditorDrawerService());
            window.titleContent = new GUIContent("Pixenity");
            window.minSize = new Vector2(600,600);
            window.Show();
            
        }

        private void OnGUI()
        {
            presenter.OnGUI();
        }
    }
}