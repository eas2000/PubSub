using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusWrap;
using Autofac;

namespace Subscriber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMsgListener
    {
        private static IContainer Container { get; set; }
        private static IBusWrap _theBus;
        private List<PubSubMsg> _messages;

        private IBusWrap TheBus
        {
            get { return _theBus; }
            set
            {
                _theBus = value;
                onBusChanged();
            }
        }
     
        public List<Type> Buses { get; set; }
     
        public MainWindow()
        {
            InitializeComponent();
            Init();
            
        }
        void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AzureBus>().Named<IBusWrap>("AzureBus");
            builder.RegisterType<KafkaBus>().Named<IBusWrap>("KafkaBus");
            Container = builder.Build();
            var types = Container.ComponentRegistry.Registrations.Where(r => typeof(IBusWrap).IsAssignableFrom(r.Activator.LimitType)).Select(r => r.Activator.LimitType);
            List<Type> Buses = types.ToList<Type>();
            _messages = new List<PubSubMsg>();
            cbBuses.ItemsSource = Buses;
            

        }

        private void cbBuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TheBus = builder.RegisterType<SomeType>().As<IService>();;
            string SelectedType = ((sender as ComboBox).SelectedItem as Type).Name;
            if (SelectedType.Length>1)
                TheBus  = Container.ResolveNamed<IBusWrap>(SelectedType) as IBusWrap;

        }
        private void lbAllTopics_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            _theBus.Subscribe((((System.Windows.Controls.ListBoxItem)sender).Content as string));
            lbSubscriptions.ItemsSource = null;
            lbSubscriptions.ItemsSource = _theBus.GetTopicsSubscribed();
        }
        private void lbSubscriptions_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            _theBus.UnSubscribe((((System.Windows.Controls.ListBoxItem)sender).Content as string));
            lbSubscriptions.ItemsSource = null;
            lbSubscriptions.ItemsSource = _theBus.GetTopicsSubscribed();
        }

        private void onBusChanged()
        {
            cbTopics.ItemsSource = _theBus.GetTopics();
            lbAllTopics.ItemsSource = _theBus.GetTopics();
            lbSubscriptions.ItemsSource = _theBus.GetTopicsSubscribed();
            dgMessages.ItemsSource = _theBus.GetMessages(new List<string>());
            _theBus.RegisterListener(this);
            _theBus.Run_Consume();
   
        }

        private void cbTopics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                string SelectedTopic = (sender as ComboBox).SelectedItem.ToString();
                List<string> topics = new List<string>();
                topics.Add(SelectedTopic);
                dgMessages.ItemsSource = TheBus.GetMessages(topics);
            }
        }

        public void OnMsg(PubSubMsg msg)
        {
            _messages.Add(msg);
            dgMessages.ItemsSource = null;
            dgMessages.ItemsSource = _messages;
    
        }



    }
}
