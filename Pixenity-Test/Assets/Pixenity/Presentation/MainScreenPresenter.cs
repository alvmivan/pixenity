using System;

namespace Pixenity
{
    public class MainScreenPresenter
    {
        private readonly MainScreen window;
        private readonly DrawerService drawerService;
        private readonly DrawableField<bool> lines; 
        
        
        public MainScreenPresenter(MainScreen window, DrawerService drawerService)
        {
            this.window = window;
            this.drawerService = drawerService;
            lines = drawerService.CreateBool("Show Lines");
        }

        public void OnGUI()
        {
            lines.Draw();
        }
    }

    public interface DrawerService
    {
        DrawableField<bool> CreateBool(string label="");
    }

    public delegate void ValueChanged<in T>(T oldValue, T newValue);
    public interface DrawableField<T> : IDisposable    
    {
        event ValueChanged<T> OnChange ;
        T Value { get; set; }
        void Draw();
    }

    public interface MainScreen
    {
        
    }
}