using Dalamud.Game.Command;
using Dalamud.Plugin;

namespace PeeChat
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "Pee Chat";

        private static Plugin _instance;

        private DalamudPluginInterface pi;
        private Configuration configuration;
        private PluginUI ui;
        private GameClient gameClient;
        
        // When loaded by LivePluginLoader, the executing assembly will be wrong.
        // Supplying this property allows LivePluginLoader to supply the correct location, so that
        // you have full compatibility when loaded normally and through LPL.
        public string AssemblyLocation { get => assemblyLocation; set => assemblyLocation = value; }
        private string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

        #region Properties
        /// <summary>
        /// Dalamud plugin interface.
        /// </summary>
        public static DalamudPluginInterface Dalamud => Instance.pi;

        /// <summary>
        /// Game client utility functions.
        /// </summary>
        public static GameClient GameClient => Instance.gameClient;

        /// <summary>
        /// All accessible UI windows.
        /// </summary>
        public static PluginUI UI => Instance.ui;

        /// <summary>
        /// Plugin configuration.
        /// </summary>
        public static Configuration Configuration => Instance.configuration;

        /// <summary>
        /// Singleton instance access - use this instead of _instance.
        /// </summary>
        private static Plugin Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Plugin();
                }
                return _instance;
            }
        }
        #endregion

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            _instance = this;

            this.pi = pluginInterface;
            
            this.configuration = this.pi.GetPluginConfig() as Configuration ?? new Configuration();
            this.configuration.Initialize(this.pi);

            // you might normally want to embed resources and load them from the manifest stream
            this.ui = new PluginUI(this.configuration);

            this.pi.UiBuilder.OnBuildUi += DrawUI;
            this.pi.UiBuilder.OnOpenConfigUi += (sender, args) => DrawConfigUI();

            this.gameClient = new GameClient();

            this.pi.CommandManager.AddHandler("/pee", new CommandInfo(OnCommand)
            {
                HelpMessage = "Use /pee to pee chat"
            });
        }

        public void Dispose()
        {
            this.ui.Dispose();

            this.pi.CommandManager.RemoveHandler("/pee");
            this.pi.Dispose();
        }

        private void OnCommand(string command, string args)
        {
            // in response to /pee (only command) do /yell
            string peeChatString = $"/yell {args}";
            if (GameClient.GetChatVisible()) {
                GameClient.SubmitToChat(peeChatString);
            }
        }

        private void DrawUI()
        {
            this.ui.Draw();
        }

        private void DrawConfigUI()
        {
            this.ui.SettingsVisible = true;
        }
    }
}
