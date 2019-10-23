using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlFirmaElectronica.NotificacionElectronica;
using System.Diagnostics;
using System.Threading;
using AccesoDatos;

namespace ControlFirmaElectronica
{
    public partial class wfrAlerta : Form
    {
        private clsAcuerdos Acuerdos = new clsAcuerdos();
        public ConexionMySQL CConexionMySQL { get; set; }
        wfrAlerta loading = null;
        public wfrAlerta(Acuses[]  Resoluciones)
        {
            InitializeComponent();
            CargarEncabezados();

            if (Resoluciones.Length > 0)
            {
                string strSQL;
                dgAlerta.Rows.Clear();
                dgAlerta.Rows.Add(Resoluciones.Length);
                CConexionMySQL = new ConexionMySQL();
                CargarValores();
                Acuerdos.CConexionMySQL.ConnectionString = "Server=" + Acuerdos.strServidor + ";Database=" + Acuerdos.strCentro +
                   ";Uid=" + Acuerdos.strUid + ";Pwd=" + Acuerdos.strPwd + ";Connection Timeout=6000;port=" + Acuerdos.strPuerto + ";";


                CConexionMySQL.Conectar();
                int i = 0;
                foreach (DataGridViewRow row in dgAlerta.Rows)
                {
                    if (i < Resoluciones.Length)
                    {
                     string resultado = "";
                     resultado = Acuerdos.InsertarResoluciones(Resoluciones[i].Expediente, Resoluciones[i].IdentificadorNotificacion.ToString(), Resoluciones[i].Resolucion);

            //            strSQL = "insert into resoluciones_rp(resorp_numero_expe,resorp_id_Noti,resorp_uri,resorp_fecha) values " +
            //"('" + Resoluciones[i].Expediente + "','" + Resoluciones[i].IdentificadorNotificacion + "','" + Resoluciones[i].Resolucion + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "')";
            //            CConexionMySQL.EjecutaComando(strSQL);
                    

                        row.Cells["Id"].Value = Resoluciones[i].IdentificadorNotificacion;
                        row.Cells["Expediente"].Value = Resoluciones[i].Expediente;
                        row.Cells["Uri"].Value = Resoluciones[i].Resolucion;
                        i++;
                    }
                }

               // CConexionMySQL.Desconectar();

            }
            else
            {
                MessageBox.Show("No se encontraron Resoluciones del RPP", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        public void CloseProgress(string frm)
        {
            try
            {
                Thread.Sleep(200);
                loading.Invoke(new Action(loading.Close));
                Application.OpenForms[frm].Focus();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "ESG02");

            }
        }
        public void CargarEncabezados()
        {
           
            DataGridViewTextBoxColumn Col1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Col2 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Col3 = new DataGridViewTextBoxColumn();

            Col1.Name = "id";
            Col1.HeaderText = "Id Notificación";
            Col1.Width = 100;
            Col1.Visible = true;

            Col2.Name = "Expediente";
            Col2.HeaderText = "Expediente";
            Col2.Width = 100;
            Col2.Visible = true;

            Col3.Name = "Uri";
            Col3.HeaderText = "Uri";
            Col3.Width = 350;
            Col3.Visible = true;


            //añadimos las Cols al datagridview
            dgAlerta.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2, Col3 });

        }


        private void CargarValores()
        {
            DataSet xmlParamentros = new DataSet();
            xmlParamentros.ReadXml(Application.StartupPath + "\\parFirma.xml");
            Acuerdos.intOpcion = int.Parse(xmlParamentros.Tables[0].Rows[0]["Opcion"].ToString());
            Acuerdos.strRuta = xmlParamentros.Tables[0].Rows[0]["Ruta"].ToString();
            Acuerdos.strCentro = xmlParamentros.Tables[0].Rows[0]["Centro"].ToString();
            Acuerdos.strServidor = xmlParamentros.Tables[0].Rows[0]["Ip"].ToString();
            Acuerdos.strPuerto = xmlParamentros.Tables[0].Rows[0]["Puerto"].ToString();
            Acuerdos.strUsuario = xmlParamentros.Tables[0].Rows[0]["Usuario"].ToString();
            Acuerdos.strNombre = xmlParamentros.Tables[0].Rows[0]["Nombre"].ToString();
            Acuerdos.strNivel = xmlParamentros.Tables[0].Rows[0]["Nivel"].ToString();
            Acuerdos.strMunicipio = xmlParamentros.Tables[0].Rows[0]["PartidoJudicial"].ToString();
            Acuerdos.strNombreJuzgado = xmlParamentros.Tables[0].Rows[0]["Juzgado"].ToString();
            Acuerdos.strUid = xmlParamentros.Tables[0].Rows[0]["Uid"].ToString();
            Acuerdos.strPwd = xmlParamentros.Tables[0].Rows[0]["Pwd"].ToString();
            xmlParamentros.Dispose();
        }
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            int i= 0;
            string valor;
            string strSQL = "";
            foreach (DataGridViewRow row in dgAlerta.Rows)
            {

            valor = row.Cells["id"].Value.ToString();
           
                if ( Acuerdos.ObtenerResolucion(long.Parse(valor)) == true)
                {
                    linkImprimir.Links.Remove(linkImprimir.Links[0]);
                    linkImprimir.Links.Add(0, linkImprimir.Text.Length, Acuerdos.strURL);
                    LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkImprimir.Links[0]);
                    linkImprimir_LinkClicked(null, x);
                    strSQL = "update resoluciones_rp set resorp_fecha_imp= '" + DateTime.Now.ToString("yyyy/MM/dd") + "' , resorp_hora = '" + DateTime.Now.ToString("HH:mm:ss") + "' where resorp_id_Noti ='" + valor + "'";
                    Acuerdos.ActualizarImoresionRpp(strSQL);
                }
            }

            if (MessageBox.Show("Desea volver a Reimprimir" , "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                btnImprimir_Click(sender, e);
            }
           Application.Exit();
        }

        private void linkImprimir_Click(object sender, EventArgs e)
        {

        }

        private void linkImprimir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
            Process.Start(sInfo);
        }

        private void dgAlerta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


    }
}
