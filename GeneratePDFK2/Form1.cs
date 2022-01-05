using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneratePDFK2
{
    public partial class Form1 : Form
    {
        static readonly string Salt = "testesttest1";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var password = "testtest";

            // サインアップ
            var hashStr = SignUp(password);
            Console.WriteLine(hashStr);

            // サインイン
            Console.WriteLine(SignIn(password, hashStr));

        }


        static string SignUp(string password)
        {
            return PBKDF2.Hash(password,16,10000,Salt).ToString();
        }

        static bool SignIn(string password, string hashStr)
        {
            return PBKDF2.Verify(password, hashStr);
        }
    }
}
