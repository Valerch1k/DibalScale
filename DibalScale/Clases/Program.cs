using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DibalScale.Clases
{
    class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]

        #region Dibalscop IMPORTED METHODS

        [DllImport("Dibalscop.dll")]
        static extern string ItemsSend(DibalScale[] myScales,
                                        int numberScales,
                                        DibalItem[] myItems,
                                        int numberItems,
                                        int showWindow, int closeTime);

        [DllImport("Dibalscop.dll")]
        static extern string ItemsSend2(DibalScale[] myScales,
                                        int numberScales,
                                        DibalItem2[] myItems,
                                        int numberItems,
                                        int showWindow, int closeTime);

        [DllImport("Dibalscop.dll")]
        static extern string DataSend();


        [DllImport("Dibalscop.dll")]
        static extern string DataSend2();



        [DllImport("Dibalscop.dll")]
        static extern int ReadRegister(ref int serverHandle,
                                        byte[] register,
                                        string scaleIpAddress,
                                        int scalePortTx,
                                        string pcIpAddress,
                                        int pcPorRx,
                                        int timeOut,
#if UNICODE
										[MarshalAs(UnmanagedType.LPWStr)] //Manage as wide string
#endif
 string pathLogs);

        [DllImport("Dibalscop.dll")]
        static extern int CancelReadRegister(ref int serverHandle,
#if UNICODE
												[MarshalAs(UnmanagedType.LPWStr)] //Manage as wide string
#endif
 string pathLogs);



        #endregion

        #region ESTRUCTURES

        public struct DibalScale
        {
            public int masterAddress;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string IpAddress;
            public int txPort;
            public int rxPort;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string model;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string display;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string section;
            public int group;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string logsPath;



            public DibalScale(int _masterAddress, string _IpAddress, int _txPort, int _rxPort, string _model, string _display, string _section, int _group, string _logsPath)
            {
                this.masterAddress = _masterAddress;
                this.IpAddress = _IpAddress;
                this.txPort = _txPort;
                this.rxPort = _rxPort;
                this.model = _model;
                this.display = _display;
                this.section = _section;
                this.group = _group;
                this.logsPath = _logsPath;

            }
        }

        public struct DibalItem
        {
            public int code;
            public int directKey;
            public double price;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string itemName;
            public int type;
            public int section;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string expiryDate;
            public int alterPrice;
            public int number;
            public int priceFactor;
#if UNICODE
            [MarshalAs(UnmanagedType.LPWStr)]   //Manage as wide string
#endif
            public string textG;

            public DibalItem(int _code, int _directKey, double _price, string _name, int _type, int _section, string _expiryDate, int _alterPrice, int _number, int _priceFactor, string _textG)
            {
                this.code = _code;
                this.directKey = _directKey;
                this.price = _price;
                this.itemName = _name;
                this.type = _type;
                this.section = _section;
                this.expiryDate = _expiryDate;
                this.alterPrice = _alterPrice;
                this.number = _number;
                this.priceFactor = _priceFactor;
                this.textG = _textG;
            }
        }

        public struct DibalItem2
        {
            public char action;
            public int code;
            public int directKey;
            public double price;
#if UNICODE
           [MarshalAs(UnmanagedType.LPWStr)]   
#endif
            public string itemName;
#if UNICODE
           [MarshalAs(UnmanagedType.LPWStr)]   
#endif
            public string itemName2;
            public int type;
            public int section;
            public int labelFormat;
            public int EAN13Format;
            public int VATType;
            public double offerPrice;
#if UNICODE
           [MarshalAs(UnmanagedType.LPWStr)]   
#endif
            public string expiryDate;
#if UNICODE
           [MarshalAs(UnmanagedType.LPWStr)]   
#endif
            public string extraDate;
            public double tare;
#if UNICODE
           [MarshalAs(UnmanagedType.LPWStr)]   
#endif
            public string EANScanner;
            public int productClass;
            public int productDirectNumber;
            public int alterPrice;
#if UNICODE
           [MarshalAs(UnmanagedType.LPWStr)]   
#endif
            public string textG;

            public DibalItem2(char _action, int _code, int _directKey, double _price, string _name, string _name2, int _type, int _section, int _labelFormat, int _EAN13Format, int _VATType, double _offerPrice, string _expiryDate, string _extraDate, double _tare, string _EANScanner, int _productClass, int _productDirectNumber, int _alterPrice, string _textG)
            {
                this.action = _action;
                this.code = _code;
                this.directKey = _directKey;
                this.price = _price;
                this.itemName = _name;
                this.itemName2 = _name2;
                this.type = _type;
                this.section = _section;
                this.labelFormat = _labelFormat;
                this.EAN13Format = _EAN13Format;
                this.VATType = _VATType;
                this.offerPrice = _offerPrice;
                this.expiryDate = _expiryDate;
                this.extraDate = _extraDate;
                this.tare = _tare;
                this.EANScanner = _EANScanner;
                this.productClass = _productClass;
                this.productDirectNumber = _productDirectNumber;
                this.alterPrice = _alterPrice;
                this.textG = _textG;
            }
        }


        #endregion

        #region VARIABLES

        const string MODEL500RANGE = "500RANGE";
        const string MODELLSERIES = "LSERIES";
        static DataSet myDataSet = new DataSet();
        static SqlConnection mySqlConnection = new SqlConnection(DBConStrs.ConnectionString());

        #endregion

        #region ImportArray

        private static void ResultLog(string result, DibalScale[] myScale) 
        {
            try
            {
                int i = 0;
                int j = 0;

                if (result == "OK") 
                {
                    for (i = 0; i < myScale.Length; i++)
                        Log.Write("IPAdress : "+ myScale[i].IpAddress + " OK ") ;
                }
                else if (result == "No commL.dll")
                {
                    for (i = 0; i < myScale.Length; i++)
                        Log.Write("IPAdress : " + myScale[i].IpAddress + " No  connect commL.dll ");
                }
                else
                {
                    string[] ScalesError = result.Split(';');

                    for (i = 0; i < myScale.Length; i++)
                    {
                        string res = "";
                        for (j = 0; j < ScalesError.Length; j++)
                        {
                            if ((ScalesError[0] == "") || (ScalesError[j] == myScale[i].IpAddress))
                            {
                                res = "ERROR";
                                break;
                            }
                            else
                                res = "OK";
                        }
                        Log.Write("IPAdress : " + myScale[i].IpAddress + " " + res);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        private static DibalItem2[] GetItems(string MP) // Записываем в массив товары MarketPlace
        {
            try
            {
                SqlConnection mySqlConnection = new SqlConnection(DBConStrs.ConnectionString());
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = " execute [scales_Dibal articles]" + MP + "";
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlConnection.Open();
                int numberOfRowsItems = mySqlDataAdapter.Fill(myDataSet, "Items");
                mySqlConnection.Close();
                DataTable myDataTable = myDataSet.Tables["Items"];
                DibalItem2 item;
                DibalItem2[] myItems = new DibalItem2[numberOfRowsItems];
                ArrayList arlItem = new ArrayList();
                //Default Item variables
                char action = 'M';
                int itemCodeAux = 0;
                int itemDirectKeyAux = 0;
                double itemPriceAux = 0;
                string itemNameAux = string.Empty;
                string itemName2Aux = string.Empty;
                int itemTypeAux = 0;
                int itemSectionAux = 0;
                int labelFormat = 0;
                int EAN13Format = 0;
                int VATType = 0;
                double offerPrice = 0.0;
                string itemExpiryDaysAux = new string('0', 10);
                string itemExtraDate = new string('0', 10);
                double tare = 0.0;
                string EANScanner = string.Empty;
                int productClass = 0;
                int NRP = 0;
                int itemAlterPrice = 0;
                string itemTextG = string.Empty;

                foreach (DataRow myDataRow in myDataTable.Rows)
                {
                    //Cast DBnull of DataGrid
                    int.TryParse(myDataRow["Code"].ToString(), out itemCodeAux);

                    int.TryParse(myDataRow["DirectKey"].ToString(), out itemDirectKeyAux);

                    double.TryParse(myDataRow["Price"].ToString(), out itemPriceAux);

                    if (!string.IsNullOrEmpty(myDataRow["Name"].ToString()))
                        itemNameAux = myDataRow["Name"].ToString();

                    int.TryParse(myDataRow["Type"].ToString(), out itemTypeAux);

                    item = new DibalItem2(action, itemCodeAux, itemDirectKeyAux, itemPriceAux, itemNameAux, itemName2Aux, itemTypeAux, itemSectionAux, labelFormat, EAN13Format, VATType, offerPrice, itemExpiryDaysAux, itemExtraDate, tare, EANScanner, productClass, NRP, itemAlterPrice, itemTextG);
                    arlItem.Add(item);
                    myItems = (DibalItem2[])arlItem.ToArray(typeof(DibalItem2));
                }

                return myItems;
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Ошибка ( DibalItem2[] GetItems(string MP)) execute [scales_Dibal articles]" + MP + "");
                throw ex;
            }
            finally
            {
                if (mySqlConnection != null)
                {
                    mySqlConnection.Close();
                }
            }
        }

        private static DibalScale[] GetScales(string MP )  // Записываем в массив настройки подключения весов
        {
            try
            {
                ConfigTxt config = new ConfigTxt();
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                    mySqlCommand.CommandText = "SELECT IpAddress , ReceptionPortRx  From ScalesDibal Where fAct = 'true' and  MP = '" + MP + "' and [NumberScale] = '" + config.NumberProgramImport + "' ";
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                DataSet myDataSet = new DataSet();
                mySqlConnection.Open();
               int numberScales =  mySqlDataAdapter.Fill(myDataSet, "Scales");
                mySqlConnection.Close();
                DataTable myDataTable = myDataSet.Tables["Scales"];
                DibalScale scale;
                DibalScale[] myScales = new DibalScale[numberScales];
                ArrayList arlScale = new ArrayList();
                //Default Scale variables
                int scaleMasterAddressAux = 0;
                string scaleIpAddressAux = string.Empty;
                int scalePortRxAux = 3000;
                int scalePortTxAux = 3001;
                string scaleModelAux = MODEL500RANGE;
                string scaleDisplayAux = string.Empty;
                string scaleSectionsAux = string.Empty;
                int scaleGroupAux = 0;
                string scaleLogsPathAux = string.Empty;

                foreach (DataRow myDataRow in myDataTable.Rows)
                {
                    if (!string.IsNullOrEmpty(myDataRow["IpAddress"].ToString()))
                        scaleIpAddressAux = myDataRow["IpAddress"].ToString();
                    int.TryParse(myDataRow["ReceptionPortRx"].ToString(), out scalePortRxAux);
                    scaleModelAux = MODEL500RANGE;
                    scale = new DibalScale(scaleMasterAddressAux, scaleIpAddressAux, scalePortTxAux, scalePortRxAux, scaleModelAux, scaleDisplayAux, scaleSectionsAux, scaleGroupAux, scaleLogsPathAux);
                    arlScale.Add(scale);
                    myScales = (DibalScale[])arlScale.ToArray(typeof(DibalScale));
                }
                return myScales;
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Ошибка записи в массив настройки подключения весов  DibalScale[] GetScales(string id ) ");
                throw ex;
            }
            finally
            {
                if (mySqlConnection != null)
                {
                    mySqlConnection.Close();
                }
            }
        }

        private static void ImportDibalMP() // импортируем массивы  DibalScale DibalItem2 на весы    
        {
            try
            {
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = "SELECT Distinct MP  From ScalesDibal Where fAct = 'true'";
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                DataSet myDataSet = new DataSet();
                mySqlConnection.Open();
                int numberOfRows = mySqlDataAdapter.Fill(myDataSet, "ScaleMP");
                mySqlConnection.Close();
                DataTable myDataTable = myDataSet.Tables["ScaleMP"];
                if (numberOfRows > 0)
                {
                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        string Result = string.Empty;
                        var myScales = GetScales(myDataRow["MP"].ToString());
                        var myItems = GetItems(myDataRow["MP"].ToString());
                        Result = ItemsSend2(myScales, myScales.Length, myItems, myItems.Length, 0, 0);
                        ResultLog(Result , myScales);
                        if (Result == "ERROR" || Result == "CONN_ERROR" || Result == "")
                            break;
                    }
                }
                else
                {
                    Application.Exit();
                }

            }
            catch (SEHException ex) 
            {
                Log.Write(ex, " - Error ");
            }
            catch (Exception ex)
            {
                Log.Write(ex, " ImportDibalMP()  Scales и  Items на весы  - Error ");
                throw ex;
            }
            finally
            {
                if (mySqlConnection != null)
                {
                    mySqlConnection.Close();
                }
            }
        }

        #endregion

        static void Main()
        {

            try
            {
                Log.Write("Старт выгрузки на весы ");
                ImportDibalMP(); 
            }
            catch (SEHException ex)
            {
                Log.Write(ex, " - Error ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
