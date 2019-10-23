using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlFirmaElectronica
{
    public partial class preview : Form
    {
        private clsAcuerdos Acuerdos = new clsAcuerdos();
     
        public preview()
        {
            InitializeComponent();
        }

        public void preview_Load(object sender, EventArgs e)
        {

            //string vbuzon;

            //Acuerdos.strSQL = " SELECT buzon.buzon AS vbuzon FROM buzon WHERE buzon.buzon_indice =  '" + Acuerdos.strindice + "' AND buzon.buzon_pers_folio =  '" + Acuerdos.strpersfolio + "'";
            //Acuerdos.CConexionMySQL.EjecutaComando(Acuerdos.strSQL);


            

            //DataTableReader dtrResultado = Acuerdos.CConexionMySQL.RegresaComando(Acuerdos.strSQL);
            //while (dtrResultado.Read())
            //{
            //    vbuzon = dtrResultado[0].ToString();
            //}
            
        


        }
    }
}
