using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Shapes;

namespace DoppelPendel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Ctors
        public MainWindowViewModel()
        {
            _pendulum = new Pendulum();
        }
        #endregion Ctors

        #region BackingFields
        // Gravitation
        private double _G = 1;

        private double _l1 = 200;
        // Masse Kreis 1
        private double _m1 = 1;
        // Auslenkung von _l1 in RAD                            
        private double _phi1 = Math.PI * 0.5;
        // Winkelgeschw 1
        private double _phi1V = 0;
        // Winkelbeschl 1
        private double _phi1A = 0;

        private double _l2 = 200;
        // Masse Kreis 2
        private double _m2 = 1;
        // Auslenkung von _l2 in RAD
        private double _phi2 = Math.PI * 0.5;
        // Winkelgeschw 2       
        private double _phi2V = 0;
        // Winkelbeschl 2
        private double _phi2A = 0;

        // Radius der Gewichte
        private int _rBall = 15;

        private int _maxHistoryDots = 100;
        // Positionsverläufe der Gewichte
        private List<Ellipse> _historyBall1 = new List<Ellipse>();
        private List<Ellipse> _historyBall2 = new List<Ellipse>();

        private Line _line1 = new Line();
        private Line _line2 = new Line();
        private Ellipse _ball1 = new Ellipse();
        private Ellipse _ball2 = new Ellipse();

        private bool _canValuesChange = true;
        private double _opacity = 1;
        private double _phi1Binding = 90;
        private double _phi2Binding = 90;
        private bool _useAirResistance = true;

        Pendulum _pendulum;

        #endregion BackingFields

        #region Properties

        public bool CanValuesChange 
        { 
            get { return _canValuesChange; }
            set
            {
                _canValuesChange = value;
                notifyPropertyChanged();
            }
        }

        public double EditValueOpacity 
        { 
            get { return _opacity; }
            set
            {
                _opacity = value;
                notifyPropertyChanged();
            }
        }

        public bool UseAirResistance 
        {
            get { return _useAirResistance; }
            set
            {
                _useAirResistance = value;
                notifyPropertyChanged();
            }
        }

        public List<Ellipse> History1 
        {
            get { return _historyBall1; }
        }

        public List<Ellipse> History2
        {
            get { return _historyBall2; }
        }

        public double Gravitation 
        {
            get { return _G; }
            set
            {
                _G = value;
                notifyPropertyChanged();
            }
        }

        public Line Line1 
        {
            get { return _line1; }
            set 
            { 
                _line1 = value;
                notifyPropertyChanged();
            }
        }

        public Line Line2
        { 
            get { return _line2; } 
            set 
            { 
                _line2 = value;
                notifyPropertyChanged();
            }
        }

        public Ellipse Ball1 
        { 
            get { return _ball1; }
            set 
            { 
                _ball1 = value;
                notifyPropertyChanged();
            }
        }

        public Ellipse Ball2 
        { 
            get { return _ball2; }
            set 
            { 
                _ball2 = value;
                notifyPropertyChanged();
            }
        }

        public int BallRadius 
        {
            get { return _rBall; }
            set
            {
                _rBall = value;
                notifyPropertyChanged();
            }
        }

        public double Length1
        {
            get { return _l1; }
            set
            {
                _l1 = Math.Abs(value);
                notifyPropertyChanged();
            }
        }

        public double Mass1
        {
            get { return _m1; }
            set
            {
                _m1 = Math.Abs(value);
                notifyPropertyChanged();
            }
        }

        public double Angle1
        {
            get { return _phi1; }
            set
            {
                _phi1 = value;
                notifyPropertyChanged();
            }
        }

        public double Angle1Binding 
        {
            get { return _phi1Binding; }
            set 
            {
                _phi1Binding = value;
                Angle1 = Angle.DegToRad(value);
                notifyPropertyChanged();
            }
        }

        public double Angle1Velocity 
        {
            get { return _phi1V; }
            set
            {
                _phi1V = value;
                notifyPropertyChanged();
            }
        }

        public double Angle1Accelaration 
        {
            get { return _phi1A; }
            set
            {
                _phi1A = value;
                notifyPropertyChanged();
            }
        }

        public double Length2
        {
            get { return _l2; }
            set
            {
                _l2 = Math.Abs(value);
                notifyPropertyChanged();
            }
        }

        public double Mass2
        {
            get { return _m2; }
            set
            {
                _m2 = Math.Abs(value);
                notifyPropertyChanged();
            }
        }

        public double Angle2
        {
            get { return _phi2; }
            set
            {
                _phi2 = value;
                notifyPropertyChanged();
            }
        }

        public double Angle2Binding 
        {
            get { return _phi2Binding; }
            set
            {
                _phi2Binding = value;
                Angle2 = Angle.DegToRad(value);
                notifyPropertyChanged();
            }
        }

        public double Angle2Velocity
        {
            get { return _phi2V; }
            set
            {
                _phi2V = value;
                notifyPropertyChanged();
            }
        }

        public double Angle2Accelaration
        {
            get { return _phi2A; }
            set
            {
                _phi2A = value;
                notifyPropertyChanged();
            }
        }

        public int MaxHistoryCount
        {
            get { return _maxHistoryDots; }
            set
            {
                _maxHistoryDots = Math.Abs(value);
                notifyPropertyChanged();
            }
        }


        #endregion Properties

        #region Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Implementations

        #region Methods

        private void notifyPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void setup()
        {
            _pendulum.setup(Line1, Line2, Ball1, Ball2, BallRadius);
            calculate();
        }

        public void calculate()
        {
            var results = _pendulum.calculate(Gravitation, Mass1, Mass2, Length1, Length2, _phi1, _phi2, _phi1V, _phi2V);
            Angle1Accelaration = results[0];
            Angle2Accelaration = results[1];
        }  

        #endregion Methods
    }
}