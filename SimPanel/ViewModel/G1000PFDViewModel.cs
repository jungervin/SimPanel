using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.ViewModel
{
    public class G1000PFDViewModel : BaseViewModel
    {
        public G1000PFDViewModel() : base()
        { }


        private double FFMSOuterAngle;

        public double FMSOuterAngle
        {
            get { return FFMSOuterAngle; }
            set
            {
                FFMSOuterAngle = value;
                this.OnPropertyChanged();
            }
        }
        private double FFMSInnerAngle;
        public double FMSInnerAngle
        {
            get { return FFMSInnerAngle; }
            set
            {
                FFMSInnerAngle = value;
                this.OnPropertyChanged();
            }
        }

        private double FHDGAngle;
        public double HDGAngle
        {
            get { return FHDGAngle; }
            set
            {
                FHDGAngle = value;
                this.OnPropertyChanged();
            }
        }

    }
}
