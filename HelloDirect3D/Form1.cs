using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;

namespace HelloDirect3D
{
    public partial class Form1 : Form
    {
        private Direct3D m_oD3D;
        private Device m_oD3D_Device;
        private PresentParameters m_d3dpp;
        private byte nRedColor;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nRedColor = 0;
            if(!InitializeDirect3D())
            {
                MessageBox.Show("Failed Loading DirectX", "Error");
                Application.Exit();
                return;
            }
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            Invalidate();
            nRedColor++;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DestroyDirect3D();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            RenderScene();
        }

        private bool InitializeDirect3D()
        {
            bool bResult = false;
            m_d3dpp = new PresentParameters();
            m_d3dpp.SwapEffect = SwapEffect.Discard;
            m_d3dpp.Windowed = true;
            m_d3dpp.EnableAutoDepthStencil = false;
            try
            {
                
                m_oD3D = new Direct3D();
                var nAdapter = m_oD3D.Adapters.DefaultAdapter.Adapter;
                var d3ddm = m_oD3D.GetAdapterDisplayMode(nAdapter);
                m_d3dpp.BackBufferFormat = d3ddm.Format;
                m_oD3D_Device = new Device
                (
                    m_oD3D, nAdapter,
                    DeviceType.Hardware, 
                    Handle,
                    CreateFlags.SoftwareVertexProcessing,
                    m_d3dpp
                );
                bResult = !(m_oD3D_Device is null);
            }
            catch
            {
                bResult = false;
            }
            return bResult;
        }

        private void DestroyDirect3D()
        {
            if ( m_oD3D_Device != null )
                m_oD3D_Device.Dispose();
            if ( m_oD3D != null )
                m_oD3D.Dispose();
        }

        private void RenderScene()
        {
            if (m_oD3D is null ) return;
            if (m_oD3D_Device is null ) return;
            Color gdipColor = Color.FromArgb(nRedColor, 0, 0);
            Color4 d3dColor = new Color4(gdipColor);
            m_oD3D_Device.Clear(ClearFlags.Target, d3dColor, 0.0f, 0);
            m_oD3D_Device.BeginScene();
            m_oD3D_Device.EndScene();
            m_oD3D_Device.Present();
        }
       
    }
}
