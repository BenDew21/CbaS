using CBaSCore.Logic.UI.Controls;

namespace CBaSCore.Logic.Business.Gate_Business
{
    public class InputControlBusiness
    {
        private bool _outputting = false;

        private InputControl _control = null;
        
        public bool Outputting
        {
            get => _outputting;
            set
            {
                _outputting = value;
                UpdateOutputting();
            }
        }

        public InputControl Control
        {
            get => _control;
            set => _control = value;
        }
        
        private WireBusiness _output;

        public void ToggleOutputting()
        {
            Outputting = !Outputting;
        }
        
        public void UpdateOutputting()
        {
            if (_output != null)
            {
                _output.ToggleStatus(Outputting);
            }

            if (_control != null)
            {
                _control.UpdateOutputting();
            }
        }

        public void SetOutputWire(WireBusiness output)
        {
            _output = output;
        }
    }
}