namespace CBaSCore.Logic.Business.Gate_Business
{
    public class BusinessInputControl
    {
        private bool _outputting = false;
        
        public bool Outputting
        {
            get => _outputting;
            set
            {
                _outputting = value;
                UpdateOutputting();
            }
        }

        private BusinessWire _outputWire;
        
        public void UpdateOutputting()
        {
            if (_outputWire != null)
            {
                _outputWire.ToggleStatus(Outputting);
            }
        }

        public void SetOutputWire(BusinessWire outputWire)
        {
            _outputWire = outputWire;
        }
    }
}