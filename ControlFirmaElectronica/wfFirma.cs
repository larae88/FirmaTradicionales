using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AccesoDatos;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.Web.Services.Protocols;
using APISeguriSign;
using System.Diagnostics;
using System.Security.Cryptography;
using SeguriSIGNP;
using System.Security.Cryptography.X509Certificates;
using System.Net.Mail;
using System.Security.Cryptography.Pkcs;
using SistemaSC;
using ControlFirmaElectronica.NotificacionElectronica;




namespace ControlFirmaElectronica
{
    public partial class wfFirma : Form
    {
        //private string _strConexionBD = "";
        //private ConexionMySQL Conexion = new ConexionMySQL();
        private clsAcuerdos Acuerdos = new clsAcuerdos();
        private long _IDNotificacion = 0;
        String[] filePath = null;
        String[] filePath2 = null;
        String[] nombresArchivos = null;
        string[] descripcionSeccion = new string[0];
        int[] foliosSeccion = new int[0];
        int[] IdMunicipioRP = new int[0];
        string[] descripcionRP = new string[0];
        string[] ClaveActos = new string[0];
        string[] descripcionActos = new string[0];
        int validador;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        clsAcuerdos func = new clsAcuerdos();
        string path = Application.StartupPath + "\\CertiPass.txt";

        public wfFirma()
        {
            InitializeComponent();
            try
            {

                FormatoListaAcuerdos();
                FormatoListaFirmas();
                FormatoListaNotificaciones();
                CargarValores();
                Acuerdos.CConexionMySQL.ConnectionString = "Server=" + Acuerdos.strServidor + ";Database=" + Acuerdos.strCentro +
                    ";Uid=" + Acuerdos.strUid + ";Pwd=" + Acuerdos.strPwd + ";Connection Timeout=6000;port=" + Acuerdos.strPuerto + ";";
                
                dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();

                if (File.Exists(path))
                {
                    System.IO.File.Delete(path);
                
                }
                //Datos que no se muestran al usuario
                if (Acuerdos.intOpcion == 1)
                {

                    dgAcuerdos.Columns[0].Visible = false;
                    dgAcuerdos.Columns[1].Visible = false;
                    dgAcuerdos.Columns[2].Visible = false;
                    dgAcuerdos.Columns[3].Visible = false;

                }
             
                if (Acuerdos.intOpcion == 2)
                {

                    dgAcuerdos.Columns[0].Visible = false;
                    dgAcuerdos.Columns[1].Visible = false;
                    dgAcuerdos.Columns[2].Visible = false;
                    dgAcuerdos.Columns[3].Visible = false;
                    dgAcuerdos.Columns[4].Visible = false;
                    dgAcuerdos.Columns[5].Visible = true;
                    dgAcuerdos.Columns[6].Visible = true;
                    dgAcuerdos.Columns[7].Visible = true;
                    dgAcuerdos.Columns[8].Visible = false;
                    dgAcuerdos.Columns[9].Visible = true;
                    dgAcuerdos.Columns[10].Visible = false;
                    dgAcuerdos.Columns[11].Visible = false;
                    dgAcuerdos.Columns[12].Visible = false;
                    dgAcuerdos.Columns[13].Visible = false;
                }

                if (Acuerdos.intOpcion == 3)
                {
                    dgAcuerdos.Columns[0].Visible = false;
                    dgAcuerdos.Columns[1].Visible = false;
                    dgAcuerdos.Columns[2].Visible = false;
                    dgAcuerdos.Columns[3].Visible = false;
                    dgAcuerdos.Columns[4].Visible = false;
                    dgAcuerdos.Columns[5].Visible = true;
                    dgAcuerdos.Columns[6].Visible = true;
                    dgAcuerdos.Columns[7].Visible = true;
                    dgAcuerdos.Columns[8].Visible = false;
                    dgAcuerdos.Columns[9].Visible = true;
                    dgAcuerdos.Columns[10].Visible = false;
                    dgAcuerdos.Columns[11].Visible = false;
                    dgAcuerdos.Columns[12].Visible = false;

                }

                if (Acuerdos.intOpcion == 4)
                {
                    dgAcuerdos.Columns[0].Visible = false;
                    dgAcuerdos.Columns[1].Visible = true;
                    dgAcuerdos.Columns[2].Visible = true;
                    dgAcuerdos.Columns[3].Visible = true;
                    dgAcuerdos.Columns[4].Visible = true;
                    dgAcuerdos.Columns[5].Visible = false;
                }

                if (Acuerdos.intOpcion == 6)
                {

                    dgAcuerdos.Columns[0].Visible = false;
                    dgAcuerdos.Columns[1].Visible = false;
                    dgAcuerdos.Columns[2].Visible = false;
                    dgAcuerdos.Columns[3].Visible = false;
                    dgAcuerdos.Columns[4].Visible = false;
                    dgAcuerdos.Columns[5].Visible = true;
                    dgAcuerdos.Columns[6].Visible = true;
                    dgAcuerdos.Columns[7].Visible = true;
                    dgAcuerdos.Columns[8].Visible = false;
                    dgAcuerdos.Columns[9].Visible = true;
                    dgAcuerdos.Columns[10].Visible = false;
                    dgAcuerdos.Columns[11].Visible = false;
                    dgAcuerdos.Columns[12].Visible = false;
                }

                dgAcuerdos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgAcuerdos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                bool bErrorCargandoCertificados = false;
                try
                {
                    CargarFirmas(Acuerdos.ObtenerFirmas());
                }
                catch
                {
                    bErrorCargandoCertificados = true;
                }

                if (Acuerdos.intOpcion == 7)
                {
                  
    
                }
                else
                {
                    if (bErrorCargandoCertificados == false)
                    {
                        //Opciones de la interfaz
                        if (Acuerdos.intOpcion == 1)
                        {
                            //Opcion para realizar firma
                            label2.Text = "Realizar firma del acuerdo";
                            //btnVerificar.Visible = true;
                            btnNotificar.Visible = false;
                            btnpreview.Visible = false;
                            btncertificado.Visible = true;
                        }
                        if (Acuerdos.intOpcion == 2)
                        {
                            //Opción para notificar
                            label2.Text = "Enviar notificación del acuerdo";
                            btnVerificar.Visible = false;
                            btnNotificar.Visible = true;
                            btnpreview.Visible = true;
                            //env_trasl.Visible = true;
                            // chkb_correr.Visible = true;
                            btncertificado.Visible = false;
                        }
                        if (Acuerdos.intOpcion == 3)
                        {
                            //Opción para mostrar las notificaciones realizadas
                            label2.Text = "Mostrar datos de la notificación";
                            btnRevocarFirma.Enabled = false;
                            btnVerRecibo.Enabled = true;
                            btnVerificar.Visible = false;
                            btnNotificar.Visible = false;
                            linkUrl.Enabled = false;
                            linkBoleta.Enabled = false;
                            btnVerRecibo.Visible = true;
                            btnpreview.Visible = false;
                            m_acuerdo.Visible = true;
                            m_traslado.Visible = true;
                            btncertificado.Visible = false;
                            btnBuscar.Visible = true;
                            btnCancelarFiltro.Visible = false;
                            rdExpe.Visible = true;
                            rdFechaN.Visible = true;
                            txtFolio.Visible = false;
                            txtNumeroExpe.Visible = false;
                        }
                        if (Acuerdos.intOpcion == 4)
                        {
                            //Opción para  enviar  revocar
                            label2.Text = "Revocar Número Unico de Suscriptor";
                            btnVerificar.Visible = false;
                            btnNotificar.Visible = false;
                            cbFirmasDisponibles.Enabled = false;
                            lvAcuerdos.Enabled = false;
                            btnVerTextoResolutivo.Enabled = false;
                            lvFirmas.Enabled = false;
                            btnRevocarFirma.Enabled = false;
                            lvDatosNotificacion.Enabled = false;
                            btnenviarRev.Visible = true;
                            txtNumeroExpe.Enabled = false;
                            btnFiltrar.Enabled = false;
                            btnCancelarFiltro.Enabled = false;
                            btnpreview.Visible = false;

                        }
                        if (Acuerdos.intOpcion == 5)
                        {
                            //Opción para notificar el documento
                            label2.Text = "Enviar notificación del acuerdo";
                            btnVerificar.Visible = false;
                            btnNotificar.Visible = true;
                            btnenviarRev.Visible = false;
                            btnpreview.Visible = false;
                        }
                        if (Acuerdos.intOpcion == 6)
                        {
                            //Opción imprimir la boleta de registros

                            label2.Text = "Acuse de Ingreso y Resolución de Registros Públicos";
                            btnVerificar.Visible = false;
                            btnVerRecibo.Visible = true;
                            if (Acuerdos.strTipoNoti == "1" || Acuerdos.strTipoNoti == "3")
                            {
                                btnVerRecibo.Enabled = true;
                            }
                            btnVerRecibo.Enabled = false;
                            btnenviarRev.Visible = false;
                            btnRevocarFirma.Visible = false;
                            btnNotificar.Visible = false;
                            btnpreview.Visible = false;
                            btn_boleta.Visible = true;
                            linkUrl.Enabled = false;
                            btnCancelarFiltro.Visible = false;
                            btnFiltrar.Visible = false;
                            btnBuscar.Visible =true;
                            txtNumeroExpe.Visible = false;
                            //txtFolio.Visible = true;
                            dtFecha.Visible = true;
                            lblExp.Visible = false;
                            rdIngreso.Visible = true;
                            rdSolucion.Visible = true;
                            //lblNotificacion.Visible = true;
                        }
                    }
                    else
                    {
                        //Error cuando se cargan los certificados
                        label2.Text = "No se han encontrado certificados";
                        gbFirmas.Enabled = false;
                        gbAcuerdos.Enabled = false;
                        gbFirma.Enabled = false;
                        gbDatosNotificacion.Enabled = false;
                        btnVerificar.Visible = false;
                        btnNotificar.Visible = false;
                        linkUrl.Enabled = false;
                        dgAcuerdos.Enabled = false;
                        gbFiltro.Enabled = false;
                        btnSalir.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void wfFirma_Load(object sender, EventArgs e)
        {
            //timer1.Enabled = true;

           
           
        }

        //Método para mandar llamar a firmar y autenticar
        private void btnVerificar_Click(object sender, EventArgs e)
        {

            string accesofirma;

            accesofirma = ObtenerHuella();
            Acuerdos.strSQL = "SELECT vinculacion.huella_vinculacion as huella FROM vinculacion WHERE vinculacion.cargo_vinculacion =  '" + Acuerdos.strNivel + "' AND  vinculacion.status_vinculacion =  '" + 1 + "'";
            Acuerdos.CConexionMySQL.EjecutaComando(Acuerdos.strSQL);

            string _huellatoken;

            DataTableReader dtrResultado = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);
            if (dtrResultado.Read() == false)
            {
                MessageBox.Show("No se ha vinculado el token", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }
            else
            {
                while (dtrResultado.Read())
                {
                    _huellatoken = dtrResultado[0].ToString();
                }

                if (accesofirma != dtrResultado[0].ToString())
                {
                    MessageBox.Show("El Token no tiene vinculación con la cuenta de usuario.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (dtrResultado[0].ToString() == null)
                {
                    MessageBox.Show("El Token no se ha vinculado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {
                    if (cbFirmasDisponibles.SelectedIndex >= 0)
                    {
                        if (dgAcuerdos.Rows.Count > 0)
                        {
                            if (dgAcuerdos.SelectedRows.Count > 0)
                            {
                                Acuerdos.intTipoFirma = 1;
                                if (ContinuarFirmaExistente() == true)
                                {
                                    //Checar que el acuerdo ya este revizado
                                    if (Acuerdos.bAcuerdoRevizado == true)
                                    {
                                        if (Acuerdos.AbrirTextoResolutivo(0) == true)
                                        {
                                            //Inicia espera
                                            btnVerificar.Enabled = false;
                                            Cursor.Current = Cursors.WaitCursor;
                                            Acuerdos.FirmaSeleccionada = (myListObj)cbFirmasDisponibles.SelectedItem;
                                            Acuerdos.TotalFirmas = ObtenerTotalFirmasNotificacion();
                                            Acuerdos.HashOriginal = ObtenerHashAcuerdoSeleccionado();
                                            string Mensaje = Acuerdos.RealizarFirma();
                                            if (Acuerdos.strError != "Cancelación por parte del usuario")
                                                MessageBox.Show(Mensaje, "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            if (Acuerdos.FirmaCorrecta == true)
                                            {
                                                dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                                lvAcuerdos.Items.Clear();
                                                lvDatosNotificacion.Items.Clear();
                                                lvFirmas.Items.Clear();
                                            }
                                            //Termina espera
                                            Cursor.Current = Cursors.WaitCursor;
                                            //btnVerificar.Enabled = true;
                                        }
                                        else
                                            MessageBox.Show("El formato del texto resolutivo esta incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                        MessageBox.Show("El acuerdo aún no ha sido analizado y revisado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                    MessageBox.Show("El acuerdo seleccionado ya ha sido firmado por usted.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("Debe de seleccionar un acuerdo para firmar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        //Método para obtener el HASH seleccionado
        private string ObtenerHashAcuerdoSeleccionado()
        {
            return dgAcuerdos.SelectedRows[0].Cells[3].Value.ToString();
        }

        //Método para obtener las firmas de documentos
        private int ObtenerTotalFirmasNotificacion()
        {
            int Total = 0;
            for (int intIndice = 0; intIndice < lvFirmas.Items.Count; intIndice++)
            {
                if (lvFirmas.Items[intIndice].SubItems[3].Text == "DOCUMENTO")
                    Total++;
            }
            Total++;
            return Total;
        }

        //Método para validar que ya no firme de nuevo el mismo documento
        private bool ContinuarFirmaExistente()
        {
            bool Resultado = true;
            string TipoFirma = "";
            TipoFirma = Acuerdos.strNivel == "1" ? "JUEZ" : "SECRETARIO DE ACUERDOS";

            for (int Indice = 0; Indice < lvFirmas.Items.Count; Indice++)
            {
                if (Acuerdos.strUsuario == lvFirmas.Items[Indice].SubItems[2].Text || TipoFirma == lvFirmas.Items[Indice].SubItems[1].Text)
                {
                    if (Acuerdos.intTipoFirma == 1)
                    {
                        if (lvFirmas.Items[Indice].SubItems[3].Text == "DOCUMENTO")
                            Resultado = false;
                    }
                    if (Acuerdos.intTipoFirma == 2)
                    {
                        if (lvFirmas.Items[Indice].SubItems[3].Text == "ARCHIVOS")
                            Resultado = false;
                    }
                }
            }
            return Resultado;
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

        private void CargarFirmas(List<object> Firmas)
        {
            foreach (myListObj Elemento in Firmas)
            {
                cbFirmasDisponibles.Items.Add((myListObj)Elemento);
            }
        }

        ~wfFirma()
        {
            //Conexion.Desconectar();
            //Conexion.Dispose();            
        }

        private void btnNotificar_Click(object sender, EventArgs e)
        {

            if (dgAcuerdos.SelectedRows.Count > 0)
            {
                if (Acuerdos.intOpcion == 1)
                {
                    btnVerificar.Enabled = false;
                    pictureBox10.Visible = true;
                }
                if (Acuerdos.intOpcion == 2)
                {
                    chkTraslado.Checked = false;
                    btnNotificar.Enabled = false;
                    lstadjuntar.Clear();
                    lstTrasladoRp.Clear();
                    chkNotifica.Checked = false;
                    chkTrasladoRp.Checked = false;

                    if (Acuerdos.strtipoPers == "19" || Acuerdos.strtipoPers == "20" || Acuerdos.strtipoPers == "22" || Acuerdos.strtipoPers == "23")
                    {
                        pcEnviar.Visible = true;
                        dgActos.Rows.Clear();
                        cmbDestino.Enabled = false;
                        CargarEncabezados();
                      //  cmbActo.Enabled = true;
                        cmbSeccion.Enabled = false;
                        ClaveActos = new string[0];
                        descripcionActos = new string[0];
                        chkSoli.Visible = false;
                        this.cmbSeccion.Text = "";
                        ObtenerCatalogosActos(ref ClaveActos, ref descripcionActos, 0, 1, Acuerdos.ObtenerActos(6));

                        if (ClaveActos.Length > 0)
                        {
                          //  this.cmbActo.DataSource = descripcionActos;
                           // this.cmbActo.Text = "";
                            int i = 0;
                            dgActos.Rows.Clear();
                            dgActos.Rows.Add(ClaveActos.Length - 1);
                            foreach (DataGridViewRow row in dgActos.Rows)
                            {
                                if (i < ClaveActos.Length)
                                {
                                    row.Cells["Sel"].Value = false;   
                                    row.Cells["Clave"].Value = ClaveActos[i];
                                    row.Cells["Acto"].Value = descripcionActos[i];
                                    i++;
                                }
                            }
                        }
                        ObtenerCatalogosSeccion(ref IdMunicipioRP, ref descripcionRP, 0, 1, Acuerdos.ObtenerMunicipioRP(9163));

                        if (IdMunicipioRP.Length > 0)
                        {
                            this.cmbDestino.DataSource = descripcionRP;
                            this.cmbDestino.Text = descripcionRP[0];
                        }

                        FormatoAdjuntar();
                        lbl_buz2.Text = Acuerdos.strbuzon;
                        lbl_noti2.Text = Acuerdos.strNotificable;
                        lbl_expe2.Text = Acuerdos.strNumeroExpe;

                        pnlRP.Visible = true;

                        //para leeer el password y contraseña
                       

                        int counter = 0;
                        string line;

                        // Read the file and display it line by line.  
                        string Cadena;
                        Cadena = Application.StartupPath + "\\CertiPass.txt";
                        if (File.Exists(Cadena))
                        {
                            System.IO.StreamReader file = new System.IO.StreamReader(Cadena);
                            while ((line = file.ReadLine()) != null)
                            {
                                clsAcuerdos on = new clsAcuerdos();
                                line = on.DesencryptMd5Hash(line);

                                if (counter == 0)
                                {
                                    txtCer.Text = line;
                                }
                                else
                                {
                                    txtPassCer.Text = line;
                                }
                                counter++;
                            }
                        }

                    }
                    else if (Acuerdos.strtipoPers == "21")
                    {
                        FormatoAdjuntar();
                        lbl_buz2.Text = Acuerdos.strbuzon;
                        lbl_noti2.Text = Acuerdos.strNotificable;
                        lbl_expe2.Text = Acuerdos.strNumeroExpe;
                        pcEnviar.Visible = true;
                        dgActos.Rows.Clear();
                        CargarEncabezados();
                        cmbSeccion.Enabled = true;
                        cmbDestino.Enabled = true;
                        lblDestino.Visible = true;
                        pcEnviar.Visible = true;
                        chkSoli.Visible = true;
                        IdMunicipioRP = new int[0];
                        descripcionRP = new string[0];
                        foliosSeccion = new int[0];
                        descripcionSeccion = new string[0];

                        ObtenerCatalogosSeccion(ref foliosSeccion, ref descripcionSeccion, 0, 1, Acuerdos.ObtenerSeccion());

                        if (foliosSeccion.Length > 0)
                        {
                            this.cmbSeccion.DataSource = descripcionSeccion;
                            this.cmbSeccion.Text = "";
                        }

                        DataTable Municipio = Acuerdos.ObtenerMunicipioRP(int.Parse(Acuerdos.strbuzon));
                        ObtenerCatalogosSeccion(ref IdMunicipioRP, ref descripcionRP, 0, 1, Municipio);

                        if (IdMunicipioRP.Length > 0)
                        {
                            this.cmbDestino.DataSource = descripcionRP;
                            DataRow[] fila = Municipio.Select("buzon = " + Acuerdos.strbuzon.ToString());
                            this.cmbDestino.Text = fila[0]["Registro"].ToString();
                           
                        }
                        FormatoAdjuntar();
                        lbl_buz.Text = Acuerdos.strbuzon;
                        lbl_noti.Text = Acuerdos.strNotificable;
                        lbl_expe.Text = Acuerdos.strNumeroExpe;
                        pnlRP.Visible = true;
                    }
                    else
                    {
                        cmbSeccion.Enabled = false;
                        cmbDestino.Enabled = false;
                        lblDestino.Visible = false;
                        panel4.Visible = true;
                        FormatoAdjuntar();
                        lbl_buz.Text = Acuerdos.strbuzon;
                        lbl_noti.Text = Acuerdos.strNotificable;
                        lbl_expe.Text = Acuerdos.strNumeroExpe;
                        pnlRP.Visible = false;
                        picEnviar.Visible = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Necesita Seleccionar un registro", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void dgAcuerdos_Click(object sender, EventArgs e)
        {
            if (dgAcuerdos.Rows.Count > 0)
            {
                if (dgAcuerdos.SelectedRows.Count > 0)
                {
                    Acuerdos.IdFirmaSeleccionada = long.Parse(dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString());

                    if (Acuerdos.intOpcion == 1)
                    {
                        CargarValoresAcuerdos();
                    }
                    
                    if (Acuerdos.intOpcion == 2)
                    {
                        Acuerdos.strpersfolio = dgAcuerdos.SelectedRows[0].Cells[10].Value.ToString();
                        CargarValoresAcuerdosN();
                    }

                    if (Acuerdos.intOpcion == 3)
                    {
                        CargarValoresAcuerdosVN();
                       
                    }
                    if (Acuerdos.intOpcion == 4)
                    {
                        CargarValoresAcuerdosR();
                    }

                    if (Acuerdos.intOpcion == 6)
                    {
                        CargarValoresAcuerdosVNR();
                        if (Acuerdos.strTipoNoti == "1" || Acuerdos.strTipoNoti == "3")
                        {
                            btnVerRecibo.Enabled = true;
                        }
                        else
                        {
                            btnVerRecibo.Enabled = false;
                        }

                    }

                    DataTableReader Resultado = Acuerdos.CargarDescripcionAcuerdo().CreateDataReader();
                    lvAcuerdos.Items.Clear();

                    //Cargar los datos del acuerdo
                    int i = 0;
                    while (Resultado.Read())
                    {
                        for (int cont = 0; cont <= Resultado.FieldCount - 1; cont++)
                        {
                            if (Resultado.GetName(cont) != "IdAuto")
                            {
                                ListViewItem List;
                                List = lvAcuerdos.Items.Add(Resultado.GetName(cont));
                                if (Resultado.GetName(cont) == "FechaAcue")
                                    List.SubItems.Add(DateTime.Parse(Resultado[cont].ToString()).ToString("dd/MM/yyyy"));
                                else
                                    List.SubItems.Add(Resultado[cont].ToString());
                            }
                            i += 1;
                        }
                    }
                    //Cargar los datos de la firma
                    CargarFirmasDelAcuerdo();
                    //Cargar los datos de la notificación
                    CargarDatosNotificacion();

                    //Nuevo RPP
                    if (Acuerdos.intOpcion == 6)
                    {
                        DataTable reso;
                        reso = Acuerdos.ObtenerBoletaResolucion("5", dtFecha.Value.ToString("yyyy/MM/dd"), _IDNotificacion.ToString());

                        if (reso.Rows.Count > 0)
                        {
                            btn_Resolucion.Visible = true;
                        }
                        else
                        {
                            btn_Resolucion.Visible = false;
                        }

                    }

                    //Habilitar o deshabilitar las opciones para firmar el video
                    if (Acuerdos.lngIdAuto == 1090 || Acuerdos.lngIdAuto == 1091)
                    {
                        btnenviarRev.Enabled = true;
                        //Checar que se pueda notificar o nó el auto cuando sea la opción 5
                        if (btnNotificar.Visible == true && Acuerdos.intOpcion == 5)
                        {
                            if (ObtenerTotalFirmaArchivos() == 2)
                            {
                                btnNotificar.Enabled = true;
                            }
                            else
                            {
                                btnNotificar.Enabled = false;
                                MessageBox.Show("Este acuerdo no puede ser notificado por no tener las dos firmas en los archivos de video.", "Mensaje del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                        btnenviarRev.Enabled = true;
                }
                else
                    if (Acuerdos.intOpcion == 4)
                    {
                        MessageBox.Show("Debe de seleccionar un Número Unico de Suscriptor a Revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Debe de seleccionar un acuerdo a notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
            }
        }

        //Método para cargar las firmas del acuerdo seleccionado
        private void CargarFirmasDelAcuerdo()
        {
            lvFirmas.Items.Clear();
            //Cargar los valores de las firmas
            DataTableReader ResultadoFirmas = Acuerdos.CargarFirmasAcuerdo(Acuerdos.IdFirmaSeleccionada).CreateDataReader();
            while (ResultadoFirmas.Read())
            {
                ListViewItem ListF;
                ListF = lvFirmas.Items.Add(ResultadoFirmas[0].ToString());
                ListF.SubItems.Add(ResultadoFirmas[1].ToString());
                ListF.SubItems.Add(ResultadoFirmas[2].ToString());
                ListF.SubItems.Add(ResultadoFirmas[3].ToString());
            }
        }

        //Método para cargar los datos de notificación
        private void CargarDatosNotificacion()
        {
            DataTableReader Resultado = Acuerdos.CargarDescripcionNotificacion().CreateDataReader();
            lvDatosNotificacion.Items.Clear();

            //Cargar los datos del acuerdo
            int i = 0;
            while (Resultado.Read())
            {
                for (int cont = 0; cont <= Resultado.FieldCount - 1; cont++)
                {
                    ListViewItem List;
                    List = lvDatosNotificacion.Items.Add(Resultado.GetName(cont));
                    if (Resultado.GetName(cont) == "FechaNotificacion")
                    {
                        if (Resultado[cont].ToString() == "0000-00-00")
                            List.SubItems.Add("");
                        else
                            List.SubItems.Add(DateTime.Parse(Resultado[cont].ToString()).ToString("dd/MM/yyyy"));
                    }


                    if (Resultado.GetName(cont) == "IdNotificacion")
                    {
                        _IDNotificacion = long.Parse(Resultado[cont].ToString());
                        List.SubItems.Add(Resultado[cont].ToString());
                    }

                    if (Resultado.GetName(cont) == "Traslado")
                    {
                        List.SubItems.Add(Resultado[cont].ToString());
                        if (Resultado[cont].ToString() == "SI")
                        {
                            m_traslado.Enabled = true;
                        }
                        else
                        {
                            m_traslado.Enabled = false;
                        }
                    }
                    i += 1;
                }
            }
        }

        //Método para cargar las variables con el auto seleccionado
        private void CargarValoresAcuerdos()
        {
            Acuerdos.strTipoExpe = dgAcuerdos.SelectedRows[0].Cells[1].Value.ToString();
            Acuerdos.strNumeroExpe = dgAcuerdos.SelectedRows[0].Cells[5].Value.ToString();
            Acuerdos.strTipoMovi = dgAcuerdos.SelectedRows[0].Cells[2].Value.ToString();
            Acuerdos.strFolioMovi = dgAcuerdos.SelectedRows[0].Cells[7].Value.ToString();

        }

        public void CargarValoresAcuerdosN()
        {
            Acuerdos.strTipoExpe = dgAcuerdos.SelectedRows[0].Cells[1].Value.ToString();
            Acuerdos.strTipoMovi = dgAcuerdos.SelectedRows[0].Cells[2].Value.ToString();
            Acuerdos.strevidencia = dgAcuerdos.SelectedRows[0].Cells[3].Value.ToString();
            Acuerdos.strNumeroExpe = dgAcuerdos.SelectedRows[0].Cells[5].Value.ToString();
            Acuerdos.strFolioMovi = dgAcuerdos.SelectedRows[0].Cells[7].Value.ToString();
            Acuerdos.strNotificable = dgAcuerdos.SelectedRows[0].Cells[9].Value.ToString();
            Acuerdos.strpersfolio = dgAcuerdos.SelectedRows[0].Cells[10].Value.ToString();
            Acuerdos.strindice = dgAcuerdos.SelectedRows[0].Cells[11].Value.ToString();
            Acuerdos.strbuzon = dgAcuerdos.SelectedRows[0].Cells[12].Value.ToString();
            Acuerdos.strtipoPers = dgAcuerdos.SelectedRows[0].Cells[13].Value.ToString();
           
        }


        public void CargarValoresAcuerdosVN()
        {
            Acuerdos.strTipoExpe = dgAcuerdos.SelectedRows[0].Cells[1].Value.ToString();
            Acuerdos.strNumeroExpe = dgAcuerdos.SelectedRows[0].Cells[5].Value.ToString();
            Acuerdos.strTipoMovi = dgAcuerdos.SelectedRows[0].Cells[2].Value.ToString();
            Acuerdos.strFolioMovi = dgAcuerdos.SelectedRows[0].Cells[7].Value.ToString();
            Acuerdos.strNotificable = dgAcuerdos.SelectedRows[0].Cells[9].Value.ToString();
            Acuerdos.strpersfolio = dgAcuerdos.SelectedRows[0].Cells[10].Value.ToString();
            Acuerdos.strbuzon = dgAcuerdos.SelectedRows[0].Cells[11].Value.ToString();
        //    Acuerdos.strTipoNoti = dgAcuerdos.SelectedRows[0].Cells[12].Value.ToString();
        }
        public void CargarValoresAcuerdosVNR()
        {
            Acuerdos.strTipoExpe = dgAcuerdos.SelectedRows[0].Cells[1].Value.ToString();
            Acuerdos.strNumeroExpe = dgAcuerdos.SelectedRows[0].Cells[5].Value.ToString();
            Acuerdos.strTipoMovi = dgAcuerdos.SelectedRows[0].Cells[2].Value.ToString();
            Acuerdos.strFolioMovi = dgAcuerdos.SelectedRows[0].Cells[7].Value.ToString();
            Acuerdos.strNotificable = dgAcuerdos.SelectedRows[0].Cells[9].Value.ToString();
            Acuerdos.strpersfolio = dgAcuerdos.SelectedRows[0].Cells[10].Value.ToString();
            Acuerdos.strbuzon = dgAcuerdos.SelectedRows[0].Cells[11].Value.ToString();
            Acuerdos.strTipoNoti = dgAcuerdos.SelectedRows[0].Cells[12].Value.ToString();
        }

        public void CargarValoresAcuerdosR()
        {
            Acuerdos.strindice = dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString();
            Acuerdos.strbuzon = dgAcuerdos.SelectedRows[0].Cells[1].Value.ToString();
            Acuerdos.strpersfolio = dgAcuerdos.SelectedRows[0].Cells[2].Value.ToString();
            Acuerdos.strpersfolioN = dgAcuerdos.SelectedRows[0].Cells[3].Value.ToString();
            Acuerdos.strNumeroexpeR = dgAcuerdos.SelectedRows[0].Cells[4].Value.ToString();
            Acuerdos.strtipoexpeR = dgAcuerdos.SelectedRows[0].Cells[5].Value.ToString();

        }


        public void CargarValoresAcuerdosPrev()
        {
            Acuerdos.strTipoExpe = dgAcuerdos.SelectedRows[0].Cells[1].Value.ToString();
            Acuerdos.strNumeroExpe = dgAcuerdos.SelectedRows[0].Cells[5].Value.ToString();
            Acuerdos.strTipoMovi = dgAcuerdos.SelectedRows[0].Cells[2].Value.ToString();
            Acuerdos.strFolioMovi = dgAcuerdos.SelectedRows[0].Cells[7].Value.ToString();
            Acuerdos.strpersfolio = dgAcuerdos.SelectedRows[0].Cells[10].Value.ToString();
            Acuerdos.strindice = dgAcuerdos.SelectedRows[0].Cells[11].Value.ToString();
            Acuerdos.strbuzon = dgAcuerdos.SelectedRows[0].Cells[12].Value.ToString();

        }

        public void FormatoListaAcuerdos()
        {
            lvAcuerdos.View = View.Details;

            // Allow the user to edit item text.
            lvAcuerdos.LabelEdit = false;

            // Allow the user to rearrange columns.
            lvAcuerdos.AllowColumnReorder = true;

            // Select the item and subitems when selection is made.
            lvAcuerdos.FullRowSelect = true;

            // Display grid lines.
            lvAcuerdos.GridLines = true;

            lvAcuerdos.Columns.Add("Dato", 100, HorizontalAlignment.Left);
            lvAcuerdos.Columns.Add("Descripción", 320, HorizontalAlignment.Left);
        }

        private void FormatoListaFirmas()
        {
            lvFirmas.View = View.Details;

            // Allow the user to edit item text.
            lvFirmas.LabelEdit = false;

            // Allow the user to rearrange columns.
            lvFirmas.AllowColumnReorder = true;

            // Select the item and subitems when selection is made.
            lvFirmas.FullRowSelect = true;

            // Display grid lines.
            lvFirmas.GridLines = true;

            lvFirmas.Columns.Add("IdUsuario", 0, HorizontalAlignment.Left);
            lvFirmas.Columns.Add("Firmas", 220, HorizontalAlignment.Left);
            lvFirmas.Columns.Add("Usuario", 0, HorizontalAlignment.Left);
            lvFirmas.Columns.Add("Tipo", 200, HorizontalAlignment.Left);
        }


        public void FormatoAdjuntar()
        {
            lstadjuntar.View = View.Details;

            // Allow the user to edit item text.
            lstadjuntar.LabelEdit = false;

            // Allow the user to rearrange columns.
            lstadjuntar.AllowColumnReorder = true;

            // Select the item and subitems when selection is made.
            lstadjuntar.FullRowSelect = true;

            // Display grid lines.
            lstadjuntar.GridLines = true;


            lstadjuntar.Columns.Add("Archivos Adjuntos", 340, HorizontalAlignment.Left);

        }

        public void FormatoVertras()
        {
            lstverTras.View = View.Details;

            // Allow the user to edit item text.
            lstverTras.LabelEdit = false;

            // Allow the user to rearrange columns.
            lstverTras.AllowColumnReorder = true;

            // Select the item and subitems when selection is made.
            lstverTras.FullRowSelect = true;

            // Display grid lines.
            lstverTras.GridLines = true;


            lstverTras.Columns.Add("Archivos Adjuntos", 340, HorizontalAlignment.Left);
            lstverTras.Columns.Add("count", 0, HorizontalAlignment.Left);


        }


        private void FormatoListaNotificaciones()
        {
            lvDatosNotificacion.View = View.Details;

            // Allow the user to edit item text.
            lvDatosNotificacion.LabelEdit = false;

            // Allow the user to rearrange columns.
            lvDatosNotificacion.AllowColumnReorder = true;

            // Select the item and subitems when selection is made.
            lvDatosNotificacion.FullRowSelect = true;

            // Display grid lines.
            lvDatosNotificacion.GridLines = true;

            lvDatosNotificacion.Columns.Add("Dato", 100, HorizontalAlignment.Left);
            lvDatosNotificacion.Columns.Add("Descripción", 320, HorizontalAlignment.Left);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgAcuerdos.Rows.Count > 0)
            {
                if (dgAcuerdos.SelectedRows.Count > 0)
                {
                    if (Acuerdos.AbrirTextoResolutivo(1) == false)
                        MessageBox.Show("Formato de texto resolutivo incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Debe de seleccionar un acuerdo para ver su texto resolutivo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("No hay acuerdos para mostrar el texto resolutivo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnRevocarFirma_Click(object sender, EventArgs e)
        {
            string accesofirma;


            accesofirma = ObtenerHuella();

            Acuerdos.strSQL = "SELECT vinculacion.huella_vinculacion as huella FROM vinculacion WHERE vinculacion.cargo_vinculacion =  '" + Acuerdos.strNivel + "' AND  vinculacion.status_vinculacion =  '" + 1 + "'";
            Acuerdos.CConexionMySQL.EjecutaComando(Acuerdos.strSQL);


            string _huellatoken;

            DataTableReader dtrResultado = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);
            if (dtrResultado.Read() == false)
            {
                MessageBox.Show("No se ha vinculado el token", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                while (dtrResultado.Read())
                {
                    _huellatoken = dtrResultado[0].ToString();
                }


                if (accesofirma != dtrResultado[0].ToString())
                {
                    MessageBox.Show("El Token no tiene vinculación con la cuenta de usuario.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (dtrResultado[0].ToString() == null)
                {
                    MessageBox.Show("El Token no se ha vinculado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {

                    if (lvFirmas.Items.Count > 0)
                    {
                        if (lvFirmas.SelectedItems.Count > 0)
                        {
                            if (Acuerdos.strUsuario == lvFirmas.SelectedItems[0].SubItems[2].Text)
                            {
                                if (cbFirmasDisponibles.SelectedIndex >= 0)
                                {
                                    if (MessageBox.Show("Esta seguro que desea revocar la firma.", "Mensaje del sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        //Verificar que la firma sea igual
                                        Acuerdos.FirmaSeleccionada = (myListObj)cbFirmasDisponibles.SelectedItem;
                                        Acuerdos.intTipoFirma = ObtenerTipoFirma();
                                        //Inicia espera
                                        btnRevocarFirma.Enabled = false;
                                        Cursor.Current = Cursors.WaitCursor;
                                        if (Acuerdos.intTipoFirma == 1)
                                        {
                                            //Verificar cuando sea un documento
                                            if (Acuerdos.CompararFirmaElectronica() == true)
                                            {
                                                if (Acuerdos.RevocarFirma(long.Parse(lvFirmas.SelectedItems[0].Text)) == true)
                                                {
                                                    CargarFirmasDelAcuerdo();
                                                    dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                                    MessageBox.Show("Firma electrónica avanzada revocada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                            else
                                                MessageBox.Show("La comparación de la firma del acuerdo seleccionado con la firma guardada anteriormente no son idénticas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                        else
                                        {
                                            //Verificar cuando sea el esquema de los archivos
                                            if (System.IO.File.Exists(Acuerdos.strRuta + "\\firmaele\\HuellasDigitales.xml"))
                                                System.IO.File.Delete(Acuerdos.strRuta + "\\firmaele\\HuellasDigitales.xml");

                                            Acuerdos.ObtenerEsquemaArchivos().WriteXml(Acuerdos.strRuta + "\\firmaele\\HuellasDigitales.xml");
                                            if (Acuerdos.CompararFirmaElectronica(Acuerdos.strRuta + "\\firmaele\\HuellasDigitales.xml") == true)
                                            {
                                                if (Acuerdos.RevocarFirma(long.Parse(lvFirmas.SelectedItems[0].Text)) == true)
                                                {
                                                    CargarFirmasDelAcuerdo();
                                                    dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                                    MessageBox.Show("Firma electrónica avanzada revocada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                            else
                                                MessageBox.Show("La comparación de la firma de los archivos comparada con la firma guardada anteriormente no son idénticas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                        //Fin de espera
                                        btnRevocarFirma.Enabled = true;
                                        Cursor.Current = Cursors.Default;
                                    }
                                }
                                else
                                    MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("La firma que desea revocar no es la de usted.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            MessageBox.Show("Debe de seleccionar una firma para revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show("Debe de seleccionar una firma para revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private int ObtenerTipoFirma()
        {
            int Resultado = 0;
            if (lvFirmas.SelectedItems[0].SubItems[3].Text == "DOCUMENTO")
                Resultado = 1;
            else
                Resultado = 2;
            return Resultado;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
            Process.Start(sInfo);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (txtNumeroExpe.Text.Trim().Length > 0)
            {
                Acuerdos.strNumeroExpeBusqueda = txtNumeroExpe.Text;
                dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
            }
            else
                MessageBox.Show("Debe de escribir un número de expediente para filtrar la información.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnCancelarFiltro_Click(object sender, EventArgs e)
        {
            Acuerdos.strNumeroExpeBusqueda = "";
            txtNumeroExpe.Text = "";
            dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
        }

        private void btnVerRecibo_Click(object sender, EventArgs e)
        {
            if (_IDNotificacion > 0)
            {
                if (Acuerdos.ObtenerRecibo(_IDNotificacion) == true)
                {
                    linkUrl.Links.Remove(linkUrl.Links[0]);
                    linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                    LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                    linkUrl_LinkClicked(null, x);
                }
                else
                    MessageBox.Show("Hubo un error al cargar el recibo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("Debe de seleccionar el acuerdo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //Método para cargar los archivos en caso de que ya exista una firma
        private void Verificar_CargarArchivosFirma(wfFirmaArchivos wFA)
        {
            for (int Indice = 0; Indice < lvFirmas.Items.Count; Indice++)
            {
                if (lvFirmas.Items[Indice].SubItems[3].Text == "ARCHIVOS")
                {
                    wFA.btnVerTextoResolutivo.Enabled = false;
                    //Actualizar solo el contador de la firma
                    wFA.intOpcionFirmar = 2;
                    wFA.CargarArchivosFirmados();
                }
            }
        }


        public string ObtenerHuella()
        {
            string _Respuesta = "";
            string huella;
            try
            {
                CspParameters csp = new CspParameters(1, "SafeSign Standard Cryptographic Service Provider");
                csp.Flags = CspProviderFlags.UseDefaultKeyContainer;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                _Respuesta = rsa.CspKeyContainerInfo.UniqueKeyContainerName;
                huella = _Respuesta;
            }
            catch
            {
            }
            return _Respuesta;

        }

        //Método para obtener el total de firmas de archivos, sirve para la opción 5
        private int ObtenerTotalFirmaArchivos()
        {
            int Resultado = 0;
            for (int Indice = 0; Indice < lvFirmas.Items.Count; Indice++)
            {
                if (lvFirmas.Items[Indice].SubItems[3].Text == "ARCHIVOS")
                {
                    Resultado++;
                }
            }
            return Resultado;
        }

        private void gbFiltro_Enter(object sender, EventArgs e)
        {

        }

        private void dgAcuerdos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lvAcuerdos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gbAcuerdos_Enter(object sender, EventArgs e)
        {

        }

        private void btnenviarRev_Click(object sender, EventArgs e)
        {

            ////Suscriptores.ContratoGestionSuscriptoresClient clientesusNR = new Suscriptores.ContratoGestionSuscriptoresClient();
            //// Suscriptores.Verificador veriNR = new Suscriptores.Verificador();
            //  clsAcuerdos objeto = new clsAcuerdos();

            // objeto.BloquearAccesoExpediente();
            //CargarValoresAcuerdosR(); 
            Acuerdos.BloquearAccesoExpediente();
            Acuerdos.EnviarRev();
            MessageBox.Show("Revocación Enviada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
        }

        private void gbDatosNotificacion_Enter(object sender, EventArgs e)
        {

        }

        private void gbNotificaciones_Enter(object sender, EventArgs e)
        {

        }

        private void gbFirma_Enter(object sender, EventArgs e)
        {

        }

        private void btnpreview_Click(object sender, EventArgs e)
        {

            if (dgAcuerdos.Rows.Count > 0)
            {
                if (dgAcuerdos.SelectedRows.Count > 0)
                {

                    CargarValoresAcuerdosPrev();

                }
            }

            preview vistaprevia = new preview();
            vistaprevia.Show();
        }

        private void btnpreview_Click_1(object sender, EventArgs e)
        {
            if (dgAcuerdos.Rows.Count > 0)
            {
                if (dgAcuerdos.SelectedRows.Count > 0)
                {
                    string _DescripcionExpediente = "";
                    string _strActor = "";
                    string _strDemandado = "";
                    string _Secretario = "";

                    Acuerdos.strSQL = "call proc_ObtenerPartes(" + Acuerdos.strTipoExpe + ",'" + Acuerdos.strNumeroExpe + "');";
                    DataTableReader dtrPartes = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);
                    while (dtrPartes.Read())
                    {
                        _DescripcionExpediente = dtrPartes["Descripcion"].ToString();
                        if (dtrPartes["Actores"].ToString().Length > 0)
                        {
                            if (dtrPartes["Actores"].ToString().EndsWith(",") == true)
                                _strActor = dtrPartes["Actores"].ToString().Substring(0, dtrPartes["Actores"].ToString().Length - 1);
                            else
                                _strActor = dtrPartes["Actores"].ToString();
                        }
                        else
                            _strActor = "";

                        if (dtrPartes["Demandados"].ToString().Length > 0)
                        {
                            if (dtrPartes["Demandados"].ToString().EndsWith(",") == true)
                                _strDemandado = dtrPartes["Demandados"].ToString().Substring(0, dtrPartes["Demandados"].ToString().Length - 1);
                            else
                                _strDemandado = dtrPartes["Demandados"].ToString();
                        }
                        else
                            _strDemandado = "";
                    }


                    Acuerdos.strSQL = "SELECT firmas.firm_vafi_id,firmas.firm_nombre as Nombre,firmas.firm_nivel as nivel FROM firmas WHERE firmas.firm_vafi_id  = '" + Acuerdos.IdFirmaSeleccionada + "' ";
                    DataTableReader dtrfirmas = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);

                    string niveljuez = "1";
                    string nivelsecre = "2";


                    while (dtrfirmas.Read())
                    {
                        _Secretario = dtrfirmas["nivel"].ToString();


                        if (_Secretario == niveljuez)
                        {
                            lbljuez.Text = dtrfirmas["Nombre"].ToString();

                        }


                        if (_Secretario == nivelsecre)
                        {
                            lblsecretario.Text = dtrfirmas["Nombre"].ToString();

                        }
                    }

                    textBox1.Text = _strActor;
                    textBox2.Text = _strDemandado;
                    lblbuzon.Text = Acuerdos.strbuzon;
                    lblnotificable.Text = Acuerdos.strNotificable;
                    lblevidencia.Text = Acuerdos.strevidencia;
                    panel3.Visible = true;
                }
                else
                {

                    MessageBox.Show("Debe de seleccionar una notificación para ver su vista previa.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("No existen notificaciónes para ver su vista previa.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);




        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void lbljuez_Click(object sender, EventArgs e)
        {

        }

       

        private void button3_Click(object sender, EventArgs e)
        {

            if (dgAcuerdos.Rows.Count > 0)
            {
                if (dgAcuerdos.SelectedRows.Count > 0)
                {

                    FormatoAdjuntar();
                    lbl_buz.Text = Acuerdos.strbuzon;
                    lbl_noti.Text = Acuerdos.strNotificable;
                    lbl_expe.Text = Acuerdos.strNumeroExpe;
                    panel4.Visible = true;

                }
                else
                {

                    MessageBox.Show("Debe de seleccionar una notificación para enviarlas y Correr traslado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("No existen notificaciones para enviarlas y Correr traslado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            lstadjuntar.Items.Clear();
            lblcantidad.Text = "0";
            btnNotificar.Enabled = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = true; //para seleccionar varios archivos a la vez
            file.Filter = "Text Files (.pdf)|*.pdf";
            file.FilterIndex = 1;
            int conteo;

            String[] nombresArchivos = null;

            if (file.ShowDialog() == DialogResult.OK)
            {
                nombresArchivos = file.SafeFileNames;
                filePath2 = file.FileNames; //guardo archivos en arreglo
            }

            if (filePath2 == null)
            {

            }

            else
            {

                foreach (string adjunto in filePath2)
                {

                    ListViewItem List;
                    List = lstadjuntar.Items.Add(adjunto);
                    conteo = lstadjuntar.Items.Count;
                    lblcantidad.Text = conteo.ToString();
                }


            }
        }



        private void lvFirmas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private async void button1_Click_1(object sender, EventArgs e)
        {
            string accesofirma;
            accesofirma = ObtenerHuella();

            Acuerdos.strSQL = "SELECT vinculacion.huella_vinculacion as huella FROM vinculacion WHERE vinculacion.cargo_vinculacion =  '" + Acuerdos.strNivel + "' AND  vinculacion.status_vinculacion =  '" + 1 + "'";
            Acuerdos.CConexionMySQL.EjecutaComando(Acuerdos.strSQL);


            string _huellatoken;

            DataTableReader dtrResultado = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);
            while (dtrResultado.Read())
            {
                _huellatoken = dtrResultado[0].ToString();
            }


            if (accesofirma != dtrResultado[0].ToString())
            {
                MessageBox.Show("El Token no tiene vinculación con la cuenta de usuario.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                if (dgAcuerdos.Rows.Count > 0)
                {
                    if (dgAcuerdos.SelectedRows.Count > 0)
                    {
                        if (cbFirmasDisponibles.SelectedIndex >= 0)
                        {
                            if (Acuerdos.intOpcion == 2)
                            {
                                CargarValoresAcuerdosN();
                            }
                            if (Acuerdos.intOpcion == 1)
                            {

                                CargarValoresAcuerdos();

                            }

                            if (Acuerdos.intOpcion == 3)
                            {
                                CargarValoresAcuerdosVN();
                            }


                            if (Acuerdos.AbrirTextoResolutivo(0) == true)
                            {
                                Acuerdos.FirmaSeleccionada = (myListObj)cbFirmasDisponibles.SelectedItem;
                                //Inicia espera
                                btnNotificar.Enabled = false;
                                Cursor.Current = Cursors.WaitCursor;

                                //if (Acuerdos.intOpcion == 2)
                                //{
                                //    CargarValoresAcuerdosR();
                                //}
                                Acuerdos.listaadj = lstadjuntar;
                                Acuerdos.corrertras = chkTraslado;
                                if (await Acuerdos.GenerarEsquemaNotificacion(long.Parse(dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString())) == true)
                                {
                                    //Actualizar las notificaciones que falta por enviar
                                    dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                    lvAcuerdos.Items.Clear();
                                    lvDatosNotificacion.Items.Clear();
                                    lvFirmas.Items.Clear();
                                    linkUrl.Links.Remove(linkUrl.Links[0]);
                                    linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                                    linkUrl.Enabled = true;


                                    MessageBox.Show("Notificación enviada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Error al enviar la notificación.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //Termina espera
                                }
                                Cursor.Current = Cursors.WaitCursor;
                                btnNotificar.Enabled = true;
                            }
                            else
                                MessageBox.Show("El formato del texto resolutivo es incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else

                        if (Acuerdos.intOpcion == 4)
                        {
                            MessageBox.Show("Debe de seleccionar un Número Unico de Suscriptor a Revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                }
                else
                    MessageBox.Show("No existen acuerdos por notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void m_acuerdo_Click(object sender, EventArgs e)
        {
            if (_IDNotificacion > 0)
            {
                if (Acuerdos.ObtenerRecibo(_IDNotificacion) == true)
                {
                    linkUrl.Links.Remove(linkUrl.Links[0]);
                    linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                    LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                    linkUrl_LinkClicked(null, x);
                }
                else
                    MessageBox.Show("Hubo un error al cargar el recibo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else


                MessageBox.Show("Debe de seleccionar el acuerdo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void m_acuerdo_Click_1(object sender, EventArgs e)
        {
            if (dgAcuerdos.Rows.Count > 0)
            {
                if (dgAcuerdos.SelectedRows.Count > 0)
                {
                    if (_IDNotificacion > 0)
                    {
                        if (Acuerdos.ObtenerAcuerdoP(_IDNotificacion) == true)
                        {
                            linkUrl.Links.Remove(linkUrl.Links[0]);
                            linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                            LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                            linkUrl_LinkClicked(null, x);
                        }
                        else
                            MessageBox.Show("Hubo un error al cargar el  Acuerdo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else


                        MessageBox.Show("Debe de seleccionar el acuerdo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {

                    MessageBox.Show("Debe de seleccionar una notificación para ver su vista previa.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("No existen notificaciónes para ver su vista previa.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);


        }

        public void lstadjuntar_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (chkb_correr.Checked == true)
            {
                env_trasl.Enabled = true;
                btnNotificar.Enabled = false;
            }
            else
            {
                env_trasl.Enabled = false;
                btnNotificar.Enabled = true;

            }

        }

        private async void pictureBox5_Click(object sender, EventArgs e)
        {
            string accesofirma;
            accesofirma = ObtenerHuella();

            Acuerdos.strSQL = "SELECT vinculacion.huella_vinculacion as huella FROM vinculacion WHERE vinculacion.cargo_vinculacion =  '" + Acuerdos.strNivel + "' AND  vinculacion.status_vinculacion =  '" + 1 + "'";
            Acuerdos.CConexionMySQL.EjecutaComando(Acuerdos.strSQL);


            string _huellatoken;

            DataTableReader dtrResultado = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);
            while (dtrResultado.Read())
            {
                _huellatoken = dtrResultado[0].ToString();
            }


            if (accesofirma != dtrResultado[0].ToString())
            {
                MessageBox.Show("El Token no tiene vinculación con la cuenta de usuario.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                if (dgAcuerdos.Rows.Count > 0)
                {
                    if (dgAcuerdos.SelectedRows.Count > 0)
                    {
                        if (cbFirmasDisponibles.SelectedIndex >= 0)
                        {
                            if (Acuerdos.intOpcion == 2)
                            {
                                CargarValoresAcuerdosN();
                            }
                            if (Acuerdos.intOpcion == 1)
                            {

                                CargarValoresAcuerdos();

                            }

                            if (Acuerdos.intOpcion == 3)
                            {
                                CargarValoresAcuerdosVN();
                            }


                            if (Acuerdos.AbrirTextoResolutivo(0) == true)
                            {
                                Acuerdos.FirmaSeleccionada = (myListObj)cbFirmasDisponibles.SelectedItem;
                                //Inicia espera
                                btnNotificar.Enabled = false;
                                Cursor.Current = Cursors.WaitCursor;
                                Acuerdos.listaadj = lstadjuntar;
                                Acuerdos.corrertras = chkTraslado;

                                if (await Acuerdos.GenerarEsquemaNotificacion(long.Parse(dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString())) == true)
                                {
                                    //Actualizar las notificaciones que falta por enviar
                                    dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                    lvAcuerdos.Items.Clear();
                                    lvDatosNotificacion.Items.Clear();
                                    lvFirmas.Items.Clear();
                                    linkUrl.Links.Remove(linkUrl.Links[0]);
                                    linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                                    linkUrl.Enabled = true;


                                    MessageBox.Show("Notificación enviada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Error al enviar la notificación.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }//Termina espera
                                Cursor.Current = Cursors.WaitCursor;
                                btnNotificar.Enabled = true;
                            }
                            else
                                MessageBox.Show("El formato del texto resolutivo es incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else

                        if (Acuerdos.intOpcion == 4)
                        {
                            MessageBox.Show("Debe de seleccionar un Número Unico de Suscriptor a Revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Debe de seleccionar un acuerdo a notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                }
                else
                    MessageBox.Show("No existen acuerdos por notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void pictureBox6_Click(object sender, EventArgs e)
        {
            // string accesofirma;


            if (txtCerti.Text == "" || txtpsw.Text == "")
            {
                MessageBox.Show("Falta agregar Certificado/Contraseña", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            else if (chkTraslado.Checked && lstadjuntar.Items.Count == 0)
            {
                MessageBox.Show(" Selecciono CorrerTraslado y no anexo ningun archivo", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                X509Certificate2 cert2 = new X509Certificate2();
                try
                {
                    X509Certificate2 cert = new X509Certificate2(filePath[0], txtpsw.Text);
                    cert2 = cert;
                    validador = 1;
                }
                catch
                {
                    MessageBox.Show("La contraseña es incorrecta", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validador = 0;
                }

                if (validador == 1)
                {
                    if (dgAcuerdos.Rows.Count > 0)
                    {
                        if (dgAcuerdos.SelectedRows.Count > 0)
                        {
                            if (cbFirmasDisponibles.Text != "")
                            {
                                if (Acuerdos.intOpcion == 2)
                                {
                                    CargarValoresAcuerdosN();

                                }
                                if (Acuerdos.intOpcion == 1)
                                {
                                    CargarValoresAcuerdos();
                                }

                                if (Acuerdos.intOpcion == 3)
                                {
                                    CargarValoresAcuerdosVN();
                                }


                                if (Acuerdos.AbrirTextoResolutivo(0) == true)
                                {
                                    Acuerdos.FirmaSeleccionada = (myListObj)cbFirmasDisponibles.SelectedItem;
                                    //Inicia espera
                                    btnNotificar.Enabled = false;
                                    Cursor.Current = Cursors.WaitCursor;
                                    Acuerdos.listaadj = lstadjuntar;
                                    Acuerdos.corrertras = chkTraslado;
                                    func.StartProgress(this);
                                    if (await Acuerdos.GenerarEsquemaNotificacion3(long.Parse(dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString()), cert2, txtCerti.Text, txtpsw.Text) == true)
                                    {
                                        //clsAcuerdos mic = new clsAcuerdos();
                                        //mic.enviartraslado(this);
                                        //Actualizar las notificaciones que falta por enviar
                                        dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                        lvAcuerdos.Items.Clear();
                                        lvDatosNotificacion.Items.Clear();
                                        lvFirmas.Items.Clear();
                                        linkUrl.Links.Remove(linkUrl.Links[0]);
                                        linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                                        linkUrl.Enabled = true;
                                        LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                                        linkUrl_LinkClicked(null, x);
                                        lstadjuntar.Items.Clear();
                                        panel4.Visible = false;
                                        chkb_correr.Checked = false;

                                        func.CloseProgress(Name);
                                        if (chkTraslado.Checked == true)
                                        {
                                            MessageBox.Show("Notificación y Traslado enviado correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Notificación y enviada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        func.CloseProgress(Name);
                                        MessageBox.Show("Error al enviar la notificación.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                        //Termina espera
                                    Cursor.Current = Cursors.WaitCursor;
                                    btnNotificar.Enabled = true;
                                }
                                else
                                    MessageBox.Show("El formato del texto resolutivo es incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else

                            if (Acuerdos.intOpcion == 4)
                            {
                                MessageBox.Show("Debe de seleccionar un Número Unico de Suscriptor a Revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Debe de seleccionar un acuerdo a notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                    }
                    else
                        MessageBox.Show("No existen acuerdos por notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            lstadjuntar.Items.Clear();
            int conteo;

            conteo = lstadjuntar.Items.Count;
            lblcantidad.Text = conteo.ToString();
        }

        private async void pictureBox5_Click_1(object sender, EventArgs e)
        {
            string accesofirma;


            if (lstadjuntar.Items.Count == 0)
            {
                MessageBox.Show("No se han anexado archivos para Correr Traslado", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            else
            {

                accesofirma = ObtenerHuella();

                Acuerdos.strSQL = "SELECT vinculacion.huella_vinculacion as huella FROM vinculacion WHERE vinculacion.cargo_vinculacion =  '" + Acuerdos.strNivel + "' AND  vinculacion.status_vinculacion =  '" + 1 + "'";
                Acuerdos.CConexionMySQL.EjecutaComando(Acuerdos.strSQL);

                string _huellatoken;

                DataTableReader dtrResultado = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);

                if (dtrResultado.Read() == false)
                {
                    MessageBox.Show("No se ha vinculado el token", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    while (dtrResultado.Read())
                    {
                        _huellatoken = dtrResultado[0].ToString();
                    }


                    if (accesofirma != dtrResultado[0].ToString())
                    {
                        MessageBox.Show("El Token no tiene vinculación con la cuenta de usuario.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    else
                    {
                        if (dgAcuerdos.Rows.Count > 0)
                        {
                            if (dgAcuerdos.SelectedRows.Count > 0)
                            {
                                if (cbFirmasDisponibles.SelectedIndex >= 0)
                                {
                                    if (Acuerdos.intOpcion == 2)
                                    {
                                        CargarValoresAcuerdosN();
                                    }
                                    if (Acuerdos.intOpcion == 1)
                                    {

                                        CargarValoresAcuerdos();
                                    }

                                    if (Acuerdos.intOpcion == 3)
                                    {
                                        CargarValoresAcuerdosVN();
                                    }

                                    if (Acuerdos.AbrirTextoResolutivo(0) == true)
                                    {
                                        Acuerdos.FirmaSeleccionada = (myListObj)cbFirmasDisponibles.SelectedItem;
                                        //Inicia espera
                                        btnNotificar.Enabled = false;
                                        Cursor.Current = Cursors.WaitCursor;

                                        Acuerdos.listaadj = lstadjuntar;
                                        Acuerdos.corrertras = chkTraslado;
                                        if (await Acuerdos.GenerarEsquemaNotificacion(long.Parse(dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString())) == true)
                                        {

                                            //Actualizar las notificaciones que falta por enviar
                                            dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                            lvAcuerdos.Items.Clear();
                                            lvDatosNotificacion.Items.Clear();
                                            lvFirmas.Items.Clear();
                                            linkUrl.Links.Remove(linkUrl.Links[0]);
                                            linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                                            linkUrl.Enabled = true;


                                            MessageBox.Show("Notificación enviada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error al enviar la notificación.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            //Termina espera
                                        }
                                        Cursor.Current = Cursors.WaitCursor;
                                        btnNotificar.Enabled = true;
                                    }
                                    else
                                        MessageBox.Show("El formato del texto resolutivo es incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                    MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else

                                if (Acuerdos.intOpcion == 4)
                                {
                                    MessageBox.Show("Debe de seleccionar un Número Unico de Suscriptor a Revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    MessageBox.Show("Debe de seleccionar un acuerdo a notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                        }
                        else
                            MessageBox.Show("No existen acuerdos por notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void m_traslado_Click(object sender, EventArgs e)
        {

            if (dgAcuerdos.Rows.Count > 0)
            {
                if (dgAcuerdos.SelectedRows.Count > 0)
                {
                    if (_IDNotificacion > 0)
                    {
                        FormatoVertras();
                        label18.Text = Acuerdos.strbuzon;
                        label19.Text = Acuerdos.strNotificable;
                        label20.Text = Acuerdos.strNumeroExpe;
                        Acuerdos.lisVertraslado = lstverTras;
                        Acuerdos.lblvertexto = lblvert;
                        if (Acuerdos.ObtenerAcuerdoT(_IDNotificacion) == true)
                        {

                        }
                        else
                            MessageBox.Show("Hubo un error al cargar el  Traslado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        this.panel5.Visible = true;

                    }
                    else

                        MessageBox.Show("Debe de seleccionar el acuerdo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {

                    MessageBox.Show("Debe de seleccionar una notificación para ver su Traslado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("No existen notificaciónes para ver su Traslado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }



        private void pictureBox5_Click_2(object sender, EventArgs e)
        {
            if (lstverTras.SelectedItems.Count > 0)
            {
                Acuerdos.lisVertraslado = lstverTras;
                if (Acuerdos.ObtenerTraslado(_IDNotificacion) == true)
                {
                    linkUrl.Links.Remove(linkUrl.Links[0]);
                    linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                    LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                    linkUrl_LinkClicked(null, x);
                }
            }
            else
            {

                MessageBox.Show("Debe de seleccionar un Archivo para verificarlo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void lstverTras_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            lstverTras.Items.Clear();
            lblvert.Text = "0";
        }

        private void lstverTras_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void btncertificado_Click(object sender, EventArgs e)
        {
            string[] descripcion = new string[0];
            int[] foliosSeccion = new int[0];
        
            if (Acuerdos.intOpcion == 1)
            {
                btnVerificar.Enabled = false;
                pictureBox10.Visible = true;
            }
            if (Acuerdos.intOpcion == 2)
            {
                btnNotificar.Enabled = false;
                picEnviar.Visible = true;
                
            }

            panel6.Visible = true;

        }

        public void ObtenerCatalogosSeccion(ref int[] I, ref string[] D, int indice1, int indice2, DataTable Seccion)
        {
            DataSet dts = new DataSet();
            dts.Tables.Add(Seccion);

            
                DataRow[] dr = Seccion.Select("");
                for (int i = 0; i < dr.Length; i++)
                {
                    Array.Resize(ref I, I.Length + 1);
                    Array.Resize(ref D, D.Length + 1);
                    I[i] = int.Parse(dr[i][indice1].ToString());
                    D[i] = dr[i][indice2].ToString().ToUpper();
                }
            

            dts.Dispose();
        }

        public void ObtenerCatalogosActos(ref string[] I, ref string[] D, int indice1, int indice2, DataTable Seccion)
        {
            DataSet dts = new DataSet();
            dts.Tables.Add(Seccion);


            DataRow[] dr = Seccion.Select("");
            for (int i = 0; i < dr.Length; i++)
            {
                Array.Resize(ref I, I.Length + 1);
                Array.Resize(ref D, D.Length + 1);
                I[i] = dr[i][indice1].ToString().ToUpper();
                D[i] = dr[i][indice2].ToString().ToUpper();
            }


            dts.Dispose();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
        }

        private void panel6_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            //file.Multiselect = true; //para seleccionar varios archivos a la vez
            file.Filter = "Text Files (.p)|*.p12";
            file.FilterIndex = 1;



            if (file.ShowDialog() == DialogResult.OK)
            {
                nombresArchivos = file.SafeFileNames;
                filePath = file.FileNames; //guardo archivos en arreglo
                lblNombre.Text = nombresArchivos[0];
                //List<object> lista = new List<object>();
                //lista.Add(nombresArchivos[0]); 
                // cbFirmasDisponibles.Items.Add(nombresArchivos[0]);
                cbFirmasDisponibles.Text = nombresArchivos[0];
                txtcertificado.Text = filePath[0];
                txtcontraseña.Focus();

            }

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

            if (Acuerdos.intOpcion == 1)
            {
                //  btnVerificar.Enabled = true;
            }
            if (Acuerdos.intOpcion == 2)
            {
                btnNotificar.Enabled = true;
            }
            panel6.Visible = false;
            txtcertificado.Text = "";
            txtcontraseña.Text = "";
            lblNombre.Text = "";

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

            if (txtcertificado.Text == "" || txtcontraseña.Text == "")
            {
                MessageBox.Show("Falta Agregar Certificado o Contraseña", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Cursor.Current = Cursors.WaitCursor;
            }

            else
            {

                X509Certificate2 cert2 = new X509Certificate2();
                try
                {

                    X509Certificate2 cert = new X509Certificate2(filePath[0], txtcontraseña.Text);
                    cert2 = cert;
                    validador = 1;
                }
                catch
                {
                    MessageBox.Show("La contraseña es incorrecta", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validador = 0;
                }

                if (validador == 1)
                {

                    if (cbFirmasDisponibles.Text != "")
                    {
                        if (dgAcuerdos.Rows.Count > 0)
                        {
                            if (dgAcuerdos.SelectedRows.Count > 0)
                            {
                                Acuerdos.intTipoFirma = 1;
                                if (ContinuarFirmaExistente() == true)
                                {
                                    //Checar que el acuerdo ya este revizado
                                    if (Acuerdos.bAcuerdoRevizado == true)
                                    {
                                        func.StartProgress(this);
                                        if (Acuerdos.AbrirTextoResolutivo(0) == true)
                                        {
                                            //Inicia espera
                                            btnVerificar.Enabled = false;
                                          
                                            Cursor.Current = Cursors.WaitCursor;
                                            //  Acuerdos.FirmaSeleccionada =  ;
                                            Acuerdos.TotalFirmas = ObtenerTotalFirmasNotificacion();
                                            Acuerdos.HashOriginal = ObtenerHashAcuerdoSeleccionado();
                                            string Mensaje = Acuerdos.RealizarFirmaCertificado(cert2);
                                            func.CloseProgress(Name);
                                            if (Acuerdos.strError != "Cancelación por parte del usuario")
                         
                                                MessageBox.Show(Mensaje, "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            if (Acuerdos.FirmaCorrecta == true)
                                            {
                                                dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                                lvAcuerdos.Items.Clear();
                                                lvDatosNotificacion.Items.Clear();
                                                lvFirmas.Items.Clear();
                                                txtcertificado.Text = "";
                                                txtcontraseña.Text = "";
                                                lblNombre.Text = "";
                                            }
                                            //Termina espera
                                            Cursor.Current = Cursors.WaitCursor;
                                            //  btnVerificar.Enabled = true;
                                           
                                        }
                                        //else
                                        //    func.CloseProgress(Name);
                                        //    MessageBox.Show("El formato del texto resolutivo esta incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                        MessageBox.Show("El acuerdo aún no ha sido analizado y revisado.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                    MessageBox.Show("El acuerdo seleccionado ya ha sido firmado por usted o ya existe una firma con el mismo perfil.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("Debe de seleccionar un acuerdo para firmar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async void picEnviar_Click(object sender, EventArgs e)
        {


            if (txtcertificado.Text == "" || txtcontraseña.Text == "")
            {
                MessageBox.Show("Falta Agregar Certificado o Contraseña", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                X509Certificate2 cert2 = new X509Certificate2();
                try
                {
                    X509Certificate2 cert = new X509Certificate2(filePath[0], txtcontraseña.Text);
                    cert2 = cert;
                    validador = 1;
                }
                catch
                {
                    MessageBox.Show("La contraseña es incorrecta", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validador = 0;
                }

                if (validador == 1)
                {
                    if (dgAcuerdos.Rows.Count > 0)
                    {
                        if (dgAcuerdos.SelectedRows.Count > 0)
                        {
                            if (cbFirmasDisponibles.Text != "")
                            {
                                if (Acuerdos.intOpcion == 2)
                                {
                                    CargarValoresAcuerdosN();
                                }
                                if (Acuerdos.intOpcion == 1)
                                {
                                    CargarValoresAcuerdos();
                                }

                                if (Acuerdos.intOpcion == 3)
                                {
                                    CargarValoresAcuerdosVN();
                                }

                                if (Acuerdos.AbrirTextoResolutivo(0) == true)
                                {
                                    func.StartProgress(this);
                                    if (await Acuerdos.GenerarEsquemaNotificacion2(long.Parse(dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString()), cert2, txtcertificado.Text, txtcontraseña.Text) == true)
                                    {
                                        Cursor.Current = Cursors.WaitCursor;
                                        //Actualizar las notificaciones que falta por enviar
                                        dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                        lvAcuerdos.Items.Clear();
                                        lvDatosNotificacion.Items.Clear();
                                        lvFirmas.Items.Clear();
                                        linkUrl.Links.Remove(linkUrl.Links[0]);
                                        linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                                        linkUrl.Enabled = true;
                                        LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                                        linkUrl_LinkClicked(null, x);

                                        if (Acuerdos.ObtenerBoleta(Acuerdos.Idnoti) == true)
                                        {
                                            linkBoleta.Links.Remove(linkBoleta.Links[0]);
                                            linkBoleta.Links.Add(0, linkBoleta.Text.Length, Acuerdos.strURL);
                                            LinkLabelLinkClickedEventArgs y = new LinkLabelLinkClickedEventArgs(linkBoleta.Links[0]);
                                            linkBoleta_LinkClicked(null, y);
                                        }

                                        txtcertificado.Text = "";
                                        txtcontraseña.Text = "";
                                        lblNombre.Text = "";
                                        func.CloseProgress(Name);
                                        MessageBox.Show("Notificación enviada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        func.CloseProgress(Name);
                                        MessageBox.Show("Error al enviar la notificación.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    //Termina espera
                                    Cursor.Current = Cursors.WaitCursor;
                                    btnNotificar.Enabled = true;
                                }
                                else
                                    MessageBox.Show("El formato del texto resolutivo es incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else

                            if (Acuerdos.intOpcion == 4)
                            {
                                MessageBox.Show("Debe de seleccionar un Número Unico de Suscriptor a Revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Debe de seleccionar un acuerdo a notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                    }
                    else
                        MessageBox.Show("No existen acuerdos por notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.panel6.Handle, 0x112, 0xf012, 0);
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.panel4.Handle, 0x112, 0xf012, 0);
        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.panel5.Handle, 0x112, 0xf012, 0);
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.panel3.Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            //file.Multiselect = true; //para seleccionar varios archivos a la vez
            file.Filter = "Text Files (.p)|*.p12";
            file.FilterIndex = 1;



            if (file.ShowDialog() == DialogResult.OK)
            {
                nombresArchivos = file.SafeFileNames;
                filePath = file.FileNames; //guardo archivos en arreglo
                lbltxtNom2.Text = nombresArchivos[0];
                //List<object> lista = new List<object>();
                //lista.Add(nombresArchivos[0]); 
                // cbFirmasDisponibles.Items.Add(nombresArchivos[0]);
                cbFirmasDisponibles.Text = nombresArchivos[0];
                txtCerti.Text = filePath[0];
                txtpsw.Focus();

            }
        }

        private void btn_boleta_Click(object sender, EventArgs e)
        {
            if (_IDNotificacion > 0)
            {
                if (Acuerdos.ObtenerBoleta(_IDNotificacion) == true)
                {
                    linkUrl.Links.Remove(linkUrl.Links[0]);
                    linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                    LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                    linkUrl_LinkClicked(null, x);
                }
                else
                    MessageBox.Show("Hubo un error al cargar la boleta.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Debe de seleccionar el acuerdo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void CargarEncabezados()
        {
            DataGridViewCheckBoxColumn Col0 = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn Col1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Col2 = new DataGridViewTextBoxColumn();
           

            Col0.Name = "Sel";
            Col0.Width = 20;
            Col0.HeaderText = "";
            Col0.Visible = true;

            Col1.Name = "Acto";
            Col1.HeaderText = "Acto";
            Col1.Width = 348;
            Col1.Visible = true;

            Col2.Name = "Clave";
            Col2.HeaderText = "Clave";
            Col2.Width = 50;
            Col2.Visible = false;

           
            //añadimos las Cols al datagridview
            dgActos.Columns.AddRange(new DataGridViewColumn[] {Col0, Col1, Col2});

        }

     
        private void btn_Resolucion_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            if (_IDNotificacion > 0)
            {
                if (Acuerdos.ObtenerResolucion(_IDNotificacion) == true)
                {
                    linkBoleta.Links.Remove(linkBoleta.Links[0]);
                    linkBoleta.Links.Add(0, linkBoleta.Text.Length, Acuerdos.strURL);
                    LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkBoleta.Links[0]);
                    linkBoleta_LinkClicked(null, x);
                    strSQL = "update resoluciones_rp set resorp_fecha_imp= '" + DateTime.Now.ToString("yyyy/MM/dd") + "' , resorp_hora_imp = '" + DateTime.Now.ToString("HH:mm:ss") + "' ,  resorp_estatus = '2'  where resorp_id_Noti ='" + _IDNotificacion + "'";
                    Acuerdos.ActualizarImoresionRpp(strSQL);
                }
                else
                    MessageBox.Show("Hubo un error al cargar la resolución.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                MessageBox.Show("Debe de seleccionar el acuerdo.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkBoleta_Click(object sender, EventArgs e)
        {

        }

        private void linkBoleta_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
            Process.Start(sInfo);
        }

        private void lblActo_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            if (Acuerdos.intOpcion == 1)
            {
                //  btnVerificar.Enabled = true;
            }
            if (Acuerdos.intOpcion == 2)
            {
                btnNotificar.Enabled = true;
            }
            pnlRP.Visible = false;
            txtCer.Text = "";
            txtPassCer.Text = "";
            lblNombreRP.Text = "";
            lstTrasladoRp.Items.Clear();
            pnlRP.Visible = false;
            chkTraslado.Checked = false;
            lblConteo.Text = "0";
            txtSolicitud.Text = "";

        }

        private async  void pcEnviar_Click(object sender, EventArgs e)
        {

            int SelActos = 0;
            int i = 0;
            string[] ClaveActosSeleccionados = new string[0];
            string[] DescripcionActosSeleccionados = new string[0];

            //verifico los seleccionados
            foreach (DataGridViewRow row in dgActos.Rows)
            {
                if (Boolean.Parse(row.Cells["Sel"].Value.ToString()))
                {
                    SelActos++;
                }
            }

            //dependiendo de los seleccionados se hace el tamaño del arreglo 
            Array.Resize(ref ClaveActosSeleccionados, SelActos);
            Array.Resize(ref DescripcionActosSeleccionados, SelActos);

            //llenamos el arreglo
            foreach (DataGridViewRow row in dgActos.Rows)
            {


                if (Boolean.Parse(row.Cells["Sel"].Value.ToString()))
                {
                    ClaveActosSeleccionados[i] = row.Cells["Clave"].Value.ToString();
                    DescripcionActosSeleccionados[i] = row.Cells["Acto"].Value.ToString();
                    i++;
                }
            }

            if (txtCer.Text == "" || txtPassCer.Text == "")
            {
                MessageBox.Show("Falta Agregar Certificado o Contraseña", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (chkTrasladoRp.Checked && lstTrasladoRp.Items.Count == 0)
            {
                MessageBox.Show(" Selecciono CorrerTraslado y no anexo ningun archivo", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (chkNotifica.Checked == false && SelActos == 0)
            {
                MessageBox.Show(" No ha seleccionado ningún acto", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                X509Certificate2 cert2 = new X509Certificate2();
                try
                {
                    X509Certificate2 cert = new X509Certificate2(filePath[0], txtPassCer.Text);
                    cert2 = cert;
                    validador = 1;
                }
                catch
                {
                    MessageBox.Show("La contraseña es incorrecta", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validador = 0;
                }

                if (validador == 1)
                {
                    if (dgAcuerdos.Rows.Count > 0)
                    {
                        if (dgAcuerdos.SelectedRows.Count > 0)
                        {
                            if (cbFirmasDisponibles.Text != "")
                            {
                                if (Acuerdos.intOpcion == 2)
                                {
                                    CargarValoresAcuerdosN();
                                    if (Acuerdos.strtipoPers == "19" || Acuerdos.strtipoPers == "20" || Acuerdos.strtipoPers == "21" || Acuerdos.strtipoPers == "22" || Acuerdos.strtipoPers == "23")
                                    {
                                        if (SelActos == 0)
                                        {
                                            if (MessageBox.Show("No ha seleccinado ningún acto ¿Está seguro de continuar con el envío?", "Mensaje del sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                            {
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }

                                        if (chkNotifica.Checked == true)
                                        {
                                            Acuerdos.strBanderaNotificacion = "1";
                                        }
                                        else
                                        {
                                            Acuerdos.strBanderaNotificacion = "0";
                                        }


                                        if (chkSoli.Checked == true && txtSolicitud.Text == "")
                                        {
                                            MessageBox.Show("Selecciono Solicitud Previa  y no se ha ingresado el Número de Solicitud Previa", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }

                                        if (Acuerdos.strtipoPers == "21")
                                        {
                                            Acuerdos.strMunicipioRp = IdMunicipioRP[cmbDestino.SelectedIndex];
                                        }
                                        if (Acuerdos.strtipoPers == "19" || Acuerdos.strtipoPers == "20" || Acuerdos.strtipoPers == "22" || Acuerdos.strtipoPers == "23")
                                        {
                                            Acuerdos.strMunicipioRp = 15;
                                        }

                                        Acuerdos.strNumeroSolicitud = txtSolicitud.Text;
                                        Acuerdos.strClaveRegistroArreglo = ClaveActosSeleccionados;
                                        //  Acuerdos.strDescripcionRegistro = cmbActo.Text;
                                        Acuerdos.strDescripcionRegistroArreglo = DescripcionActosSeleccionados;
                                    }
                                    else
                                    {
                                        Acuerdos.strClaveRegistro = "";
                                        Acuerdos.strDescripcionRegistro = "";
                                    }
                                }
                                if (Acuerdos.intOpcion == 1)
                                {

                                    CargarValoresAcuerdos();
                                }

                                if (Acuerdos.intOpcion == 3)
                                {
                                    CargarValoresAcuerdosVN();
                                }

                                if (Acuerdos.AbrirTextoResolutivo(0) == true)
                                {

                                    func.StartProgress(this);
                                    Acuerdos.listaadj = lstTrasladoRp;
                                    Acuerdos.corrertras = chkTraslado;
                                    if (await Acuerdos.GenerarEsquemaNotificacion2(long.Parse(dgAcuerdos.SelectedRows[0].Cells[0].Value.ToString()), cert2, txtCer.Text, txtPassCer.Text) == true)
                                    {

                                        Cursor.Current = Cursors.WaitCursor;
                                        //Actualizar las notificaciones que falta por enviar
                                        dgAcuerdos.DataSource = Acuerdos.ObtenerAcuerdosANotificar();
                                        lvAcuerdos.Items.Clear();
                                        lvDatosNotificacion.Items.Clear();
                                        lvFirmas.Items.Clear();

                                        if (Acuerdos.strtipoPers == "19" || Acuerdos.strtipoPers == "20" || Acuerdos.strtipoPers == "19" || Acuerdos.strtipoPers == "21" || Acuerdos.strtipoPers == "22" || Acuerdos.strtipoPers == "23")
                                        {
                                            if (Acuerdos.strBanderaNotificacion == "1")
                                            {
                                                linkUrl.Links.Remove(linkUrl.Links[0]);
                                                linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                                                linkUrl.Enabled = true;
                                                LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                                                linkUrl_LinkClicked(null, x);
                                            }
                                        }
                                        else
                                        {
                                            linkUrl.Links.Remove(linkUrl.Links[0]);
                                            linkUrl.Links.Add(0, linkUrl.Text.Length, Acuerdos.strURL);
                                            linkUrl.Enabled = true;
                                            LinkLabelLinkClickedEventArgs x = new LinkLabelLinkClickedEventArgs(linkUrl.Links[0]);
                                            linkUrl_LinkClicked(null, x);
                                        }

                                        if (SelActos > 0)
                                        {
                                            if (Acuerdos.ObtenerBoleta(Acuerdos.Idnoti) == true)
                                            {
                                                linkBoleta.Links.Remove(linkBoleta.Links[0]);
                                                linkBoleta.Links.Add(0, linkBoleta.Text.Length, Acuerdos.strURL);
                                                LinkLabelLinkClickedEventArgs y = new LinkLabelLinkClickedEventArgs(linkBoleta.Links[0]);
                                                linkBoleta_LinkClicked(null, y);
                                            }
                                        }
                                        txtcertificado.Text = "";
                                        txtcontraseña.Text = "";
                                        lblNombre.Text = "";
                                        func.CloseProgress(Name);
                                        MessageBox.Show("Notificación enviada correctamente.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        pictureBox15_Click(sender, e);

                                    }
                                    else
                                    {
                                        func.CloseProgress(Name);
                                        MessageBox.Show("Error al enviar la notificación.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    //Termina espera
                                    Cursor.Current = Cursors.WaitCursor;
                                    btnNotificar.Enabled = true;
                                }
                                else
                                    MessageBox.Show("El formato del texto resolutivo es incorrecto.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("Debe de seleccionar una firma del depósito de firmas.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else

                            if (Acuerdos.intOpcion == 4)
                            {
                                MessageBox.Show("Debe de seleccionar un Número Unico de Suscriptor a Revocar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Debe de seleccionar un acuerdo a notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                    }
                    else
                        MessageBox.Show("No existen acuerdos por notificar.", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
           
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();
            //file.Multiselect = true; //para seleccionar varios archivos a la vez
            file.Filter = "Text Files (.p)|*.p12";
            file.FilterIndex = 1;



            if (file.ShowDialog() == DialogResult.OK)
            {
                nombresArchivos = file.SafeFileNames;
                filePath = file.FileNames; //guardo archivos en arreglo
                lblNombreRP.Text = nombresArchivos[0];
                //List<object> lista = new List<object>();
                //lista.Add(nombresArchivos[0]); 
                // cbFirmasDisponibles.Items.Add(nombresArchivos[0]);
                cbFirmasDisponibles.Text = nombresArchivos[0];
                txtCer.Text= filePath[0];
                txtPassCer.Focus();

            }
        }

        private void cmbSeccion_SelectedValueChanged_1(object sender, EventArgs e)
        {

        }

        private void cmbSeccion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSeccion_SelectedValueChanged(object sender, EventArgs e)
        {

            dgActos.Rows.Clear();
            ClaveActos = new string[0];
            descripcionActos = new string[0];

            ObtenerCatalogosActos(ref ClaveActos, ref descripcionActos, 0, 1, Acuerdos.ObtenerActos(foliosSeccion[cmbSeccion.SelectedIndex]));

            if (ClaveActos.Length > 0)
            {
                int i = 0;
                dgActos.Rows.Add(ClaveActos.Length);
                foreach (DataGridViewRow row in dgActos.Rows)
                {
                    if (i < ClaveActos.Length)
                    {
                        row.Cells["Sel"].Value = false;
                        row.Cells["Clave"].Value = ClaveActos[i];
                        row.Cells["Acto"].Value = descripcionActos[i];
                        i++;
                    }
                }

                //  this.cmbActo.DataSource = descripcionActos;
                // this.cmbActo.Text = "";
               // this.dgActos.DataSource = descripcionActos;

            }
        }

        private void txtSolicitud_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
                e.Handled = false;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void chkSoli_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSoli.Checked == true)
            {
                txtSolicitud.Visible = true;
                txtSolicitud.Focus();
            }
            else
            {
                txtSolicitud.Visible = false;
                txtSolicitud.Text = "";
            }
        }

        private void dgActos_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1)
            {

                //dgvDilis.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(217, 217, 178);
                if (e.ColumnIndex == 0)
                    dgActos.CurrentRow.Cells[0].Value = true;

            }
        }

        private void dgActos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                dgActos.CurrentRow.Cells[0].Value = false;
        }

        private void lnkTraslado_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = true; //para seleccionar varios archivos a la vez
            file.Filter = "Text Files (.pdf)|*.pdf";
            file.FilterIndex = 1;
            int conteo;

            String[] nombresArchivos = null;

            if (file.ShowDialog() == DialogResult.OK)
            {
                nombresArchivos = file.SafeFileNames;
                filePath = file.FileNames; //guardo archivos en arreglo
            }

            if (filePath != null)
            {
                conteo = filePath.Length;
                lblConteo.Text = conteo.ToString();
            } 
        }

        private void chkTraslado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTraslado.Checked == true)
            {
                pctAgregar.Enabled = true;
                pctEliminar.Enabled = true;
            }
            else
            {
                pctAgregar.Enabled = false;
                pctEliminar.Enabled = false;
            }
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            lstTrasladoRp.Items.Clear();
            int conteo;

            conteo = lstTrasladoRp.Items.Count;
            lblConteo.Text = conteo.ToString();
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = true; //para seleccionar varios archivos a la vez
            file.Filter = "Text Files (.pdf)|*.pdf";
            file.FilterIndex = 1;
            int conteo;

            String[] nombresArchivos = null;

            if (file.ShowDialog() == DialogResult.OK)
            {
                nombresArchivos = file.SafeFileNames;
                filePath2 = file.FileNames; //guardo archivos en arreglo
            }

            if (filePath2 == null)
            {

            }

            else
            {

                foreach (string adjunto in filePath2)
                {

                    ListViewItem List;
                    List = lstTrasladoRp.Items.Add(adjunto);
                    conteo = lstTrasladoRp.Items.Count;
                    lblConteo.Text = conteo.ToString();
                }


            }
        }

        private void chkTrasladoRp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTrasladoRp.Checked == true)
            {
                pcAgregarRp.Enabled = true;
                pcEliminarRp.Enabled = true;
            }
            else
            {
                pcAgregarRp.Enabled = false;
                pcEliminarRp.Enabled = false;
            }
        }

        public void btnAlerta_Click(object sender, EventArgs e)
        {
            //Acuses[] Resoluciones;
            //Resoluciones = Acuerdos.ObtenerAlerta(Acuerdos.strCentro);
            //if (Resoluciones.Length > 0)
            //{
            //    wfrAlerta alerta = new wfrAlerta();
            //    this.Close();
            //    alerta.Show();
            
            //}
            //else
            //{
            //    Application.Exit();
            //}
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
          //  CargarValoresAcuerdosVN();
            string tipo = "0";

            if (Acuerdos.intOpcion == 3)
            {
                if (rdExpe.Checked == false && rdFechaN.Checked == false)
                {
                    MessageBox.Show("Debe de seleccionar una opción de búsqueda", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (Acuerdos.intOpcion == 6)
            {
                if (rdIngreso.Checked == false && rdSolucion.Checked == false)
                {
                    MessageBox.Show("Debe de seleccionar una opción de búsqueda", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (rdIngreso.Checked == true)
            {
                tipo = "1";
            }
             if (rdSolucion.Checked == true)
            {
                tipo = "2";
            }
             if (rdExpe.Checked == true)
             {
                 tipo = "3";
             }
             if (rdFechaN.Checked == true)
             {
                 tipo = "4";
             }
            dgAcuerdos.DataSource = Acuerdos.ObtenerBoletaResolucion(tipo, dtFecha.Value.ToString("yyyy/MM/dd"), txtNumeroExpe.Text);
        
            if (dgAcuerdos.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron coincidencias", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtFolio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
                e.Handled = false;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void rdExpe_CheckedChanged(object sender, EventArgs e)
        {
            txtNumeroExpe.Visible = true;
            dtFecha.Visible = false;
        }

        private void rdFechaN_CheckedChanged(object sender, EventArgs e)
        {
            txtNumeroExpe.Visible = false;
            dtFecha.Visible = true;
        }

        public string ValidaExpediente(ref TextBox txtExpediente, string strExpediente, string Juzgado)
        {
            int intDiagonal, intLongitud, intGuion;
            string strNumero, strAnio, vFormato;
            int numeroExp = 0, folioExpe = 0;
            vFormato = "";

            if (Juzgado.Contains("21103") || Juzgado.Contains("21203") || Juzgado.Contains("21102") || Juzgado.Contains("21202") || Juzgado.Contains("21201"))
            {
                //Tradicional
                intDiagonal = strExpediente.IndexOf('/');
                intLongitud = strExpediente.Length;

                if (intLongitud >= 4 && intDiagonal > 0 && intDiagonal < intLongitud && (intLongitud - intDiagonal - 1) > 1)
                {
                    if (strExpediente.IndexOf('/', intDiagonal + 1) != -1)
                    {
                        MessageBox.Show("Existe doble diagonal", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }
                }
                else
                {
                    if (intLongitud < 4)
                    {
                        MessageBox.Show("Se han introducido pocos caracteres", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }

                    else if (intDiagonal == -1)
                    {
                        MessageBox.Show("No se encuentra la diagonal", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }

                    else if ((intLongitud - intDiagonal - 1) < 2)
                    {
                        MessageBox.Show("Falta especificar el año claramente", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }

                    else if (intDiagonal == 0)
                    {
                        MessageBox.Show("Falta especificar el número a la izquierda de la diagonal", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();

                        return "";
                    }
                }
                numeroExp = int.Parse(txtExpediente.Text.Substring(0, intDiagonal));
                if (numeroExp == 0)
                {
                    MessageBox.Show("El numero de expediente no puede ser 0", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtExpediente.SelectAll();
                    //txtExpediente.Focus();
                    return "";
                }

                strNumero = string.Format("{0:d4}", int.Parse(strExpediente.Substring(0, intDiagonal)));
                strAnio = strExpediente.Substring(intDiagonal + 1);
                if (strAnio.Length < 4)
                {
                    string strAnioSistema = DateTime.Today.Year.ToString().Substring(2), strAnioNuevo = strAnio.Substring(strAnio.Length - 2);
                    int intAnio;

                    if (int.Parse(strAnioNuevo) > int.Parse(strAnioSistema))
                        intAnio = 1900 + int.Parse(strAnioNuevo);
                    else
                        intAnio = 2000 + int.Parse(strAnioNuevo);
                    strAnio = intAnio.ToString();
                }

                if (int.Parse(strAnio) > int.Parse(DateTime.Today.Year.ToString()))
                {
                    MessageBox.Show("El año del expediente no puede ser mayor al año del sistema", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "";
                }
                vFormato = string.Format("{0:d4}", int.Parse(strExpediente.Substring(0, intDiagonal))) + "/" + strAnio;
            }

            //Impugnacion,Adolescentes,Oralidad Penal
            else
            {
                intGuion = strExpediente.IndexOf('-');
                intLongitud = strExpediente.Length;

                if (intLongitud >= 6 && intGuion > 0 && intGuion < intLongitud && (intLongitud - intGuion - 1) > 1)
                {
                    if (strExpediente.IndexOf('-', intGuion + 1) != -1)
                    {
                        MessageBox.Show("Existe doble Guion", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }
                }
                else
                {
                    if (intLongitud < 6)
                    {
                        MessageBox.Show("Se han introducido pocos caracteres", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }

                    else if (intGuion == -1)
                    {
                        MessageBox.Show("No se encuentra el Guion", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }

                    else if ((intLongitud - intGuion) < 2)
                    {
                        MessageBox.Show("Falta especificar el folio claramente", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();
                        return "";
                    }

                    else if (intGuion == 0)
                    {
                        MessageBox.Show("Falta especificar el número a la izquierda del Guion", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        txtExpediente.SelectAll();
                        //txtExpediente.Focus();

                        return "";
                    }
                }

                string[] _Separador = strExpediente.Split('-');

                numeroExp = int.Parse(_Separador[0].Substring(0, 2));
                if (numeroExp == 0)
                {
                    MessageBox.Show("El municipio del expediente no puede ser 0", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtExpediente.SelectAll();
                    //txtExpediente.Focus();
                    return "";

                }

                folioExpe = int.Parse(_Separador[1].Replace("-", ""));
                if (numeroExp == 0)
                {
                    MessageBox.Show("El numero folio del expediente no puede ser 0", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtExpediente.SelectAll();
                    //txtExpediente.Focus();
                    return "";
                }
                strAnio = _Separador[0].Substring(2, 2);
                if (strAnio.Length < 2)
                {
                    string strAnioSistema = DateTime.Today.Year.ToString().Substring(2), strAnioNuevo = strAnio;
                    int intAnio;

                    if (int.Parse(strAnioNuevo) > int.Parse(strAnioSistema))
                        intAnio = 1900 + int.Parse(strAnioNuevo);
                    else
                        intAnio = 2000 + int.Parse(strAnioNuevo);
                    strAnio = intAnio.ToString();
                }

                if (int.Parse(strAnio) > int.Parse(DateTime.Today.Year.ToString()))
                {
                    MessageBox.Show("El año del expediente no puede ser mayor al año del sistema", "Favor de verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "";
                }
                //vFormato = int.Parse(strExpediente.Substring(0, intGuion)) + "-" + folioExpe;
                vFormato = strExpediente.Substring(0, intGuion) + "-" + folioExpe;
            }
            return vFormato;
        }

        public void funcionKeyPressExpe(ref TextBox txtPrueba, ref object sender, ref KeyPressEventArgs e, string Juzgado)
        {
            if (char.IsDigit(e.KeyChar))
                e.Handled = false;
            else if (e.KeyChar == '/' && txtPrueba.Text.Length < 5 && txtPrueba.Text.Length > 0 && txtPrueba.Text.IndexOf('/') == -1 && (Juzgado.Contains("21103") || Juzgado.Contains("21203") || Juzgado.Contains("21102") || Juzgado.Contains("21202") || Juzgado.Contains("21201")))//Tradicional
                e.Handled = false;
            else if (e.KeyChar == '-' && txtPrueba.Text.Length < 5 && txtPrueba.Text.Length > 0 && txtPrueba.Text.IndexOf('-') == -1 && (Juzgado.Contains("21903") || Juzgado.Contains("21303") || Juzgado.Contains("21403") || Juzgado.Contains("21404") || Juzgado.Contains("21503")))
                e.Handled = false;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnBoleta_Ing_Click(object sender, EventArgs e)
        {

        }

        private void zzz(object sender, EventArgs e)
        {

        }
    }
}










