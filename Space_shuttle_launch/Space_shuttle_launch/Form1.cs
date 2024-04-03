using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_shuttle_launch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PasswordTextBox.PasswordChar = '*';//hiding the password
        }

        //a method which finds the index of the closest to the equator location in the list of all possible spaceports
        static int FindTheClosestLocation(List<Place> spaceports)
        {
            double minLatitude = double.MaxValue;
            int indexOfTheClosest = -1; //entering an initial impossible value

            for (int i = 0; i < spaceports.Count; i++)
            {
                double absLatitude = Math.Abs(spaceports[i].latitude);
                if (absLatitude < minLatitude&&spaceports[i].theBestDayForTheLocation!="No suitable date") //check the best day in case there is no suitable date for the location
                {
                    minLatitude = absLatitude;
                    indexOfTheClosest = i;
                }
            }
            return indexOfTheClosest;
        }

        //a method which creates a csv file with all results from the spaceports
        static void SaveFinalResults(string filePath, List<Place> spacePorts)
        {
            string[][] finalResults = new string[spacePorts.Count][];//creating an array of string arrays
            for (int i = 0; i < spacePorts.Count; i++)
            {
                //saving the spaceport's name and result as one of the elements in the finalResults array
                finalResults[i] = new string[] { spacePorts[i].name, spacePorts[i].theBestDayForTheLocation};
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                //each string array element in the finalResults array to be writen in the csv file on a separate row 
                foreach (string[] row in finalResults)
                {
                    string rowString = string.Join(";", row);
                    sw.WriteLine(rowString);
                }
            }
        }

        //a method which sends an email with the results
        static void SendMail(string senderEmail,string password,string receiverEmail, string body, string filePath)
        {
            MailMessage mail = new MailMessage(senderEmail, receiverEmail);
            mail.Subject = "Space Shuttle Launch";
            mail.Body = body;
            Attachment attachment = new Attachment(filePath);
            mail.Attachments.Add(attachment);

            //working eith Outlook
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(senderEmail, password);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);

            mail.Dispose();
        }

        private void ResultButton_Click(object sender, EventArgs e)
        {
            string fileNameKFG = fpKourouTextBox.Text; //KFG.csv
            string fileNameCCU = fpCapeCanTextBox.Text; //CCU.csv
            string fileNameKU = fpKodiakTextBox.Text; //KU.csv
            string fileNameTJ = fpTanegTextBox.Text; //TJ.csv
            string fileNameMNZ = fpMahiaTextBox.Text; //MNZ.csv


            //handling errors with the file names
            if (File.Exists(fileNameKFG) && File.Exists(fileNameCCU)&& File.Exists(fileNameKU)&& File.Exists(fileNameTJ)
                && File.Exists(fileNameMNZ))
            {
                //handling the possible errors for the Kourou spaceport and geting an individual message (from 73 to 95 line)
                if (int.TryParse(KFGTextBoxTempMin.Text, out int tempMinKFG)) { }
                else
                {
                    MessageBox.Show("You should enter a number for min temperature for Kourou", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KFGTextBoxTempMin.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(KFGTextBoxTempMax.Text, out int tempMaxKFG)) 
                { 
                    if(tempMaxKFG< tempMinKFG)
                    {
                        MessageBox.Show("The max temperatures should be higher than the min ones!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        KFGTextBoxTempMax.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for max temperature for Kourou", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KFGTextBoxTempMax.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(KFGTextBoxWind.Text, out int windCriteriaKFG)) { }
                else
                {
                    MessageBox.Show("You should enter a number for the wind criteria for Kourou", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KFGTextBoxWind.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(KFGTextBoxHum.Text, out int humCriteriaKFG))
                {
                    if (humCriteriaKFG > 100 || humCriteriaKFG < 0)
                    {
                        MessageBox.Show("You should enter a number between 0 and 100 for the humidity criteria for Kourou", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        KFGTextBoxHum.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for the humidity criteria for Kourou", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KFGTextBoxHum.BackColor = Color.Yellow;
                    return;
                }




                //handling the possible errors for the Cape Canaveral spaceport and geting an individual message (from 102 to 124 line)
                if (int.TryParse(CCUTextBoxTempMin.Text, out int tempMinCCU)) { }
                else
                {
                    MessageBox.Show("You should enter a number for min temperature for Cape Canaveral", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CCUTextBoxTempMin.BackColor = Color.Yellow;
                    return;
                }

                if (int.TryParse(CCUTextBoxTempMax.Text, out int tempMaxCCU))
                {
                    if (tempMaxCCU < tempMinCCU)
                    {
                        MessageBox.Show("The max temperatures should be higher than the min ones!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CCUTextBoxTempMax.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for max temperature for Cape Canaveral", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CCUTextBoxTempMax.BackColor = Color.Yellow;
                    return;
                }

                if (int.TryParse(CCUTextBoxWind.Text, out int windCriteriaCCU)) { }
                else
                {
                    MessageBox.Show("You should enter a number for the wind criteria for Cape Canaveral", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CCUTextBoxWind.BackColor = Color.Yellow;
                    return;
                }

                if (int.TryParse(CCUTextBoxHum.Text, out int humCriteriaCCU))
                {
                    if (humCriteriaCCU > 100 || humCriteriaCCU < 0)
                    {
                        MessageBox.Show("You should enter a number between 0 and 100 for the humidity criteria for Cape Canaveral", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CCUTextBoxHum.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for the humidity criteria for Cape Canaveral", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CCUTextBoxHum.BackColor = Color.Yellow;
                    return;
                }




                //handling the possible errors for the Kodiak spaceport and geting an individual message (from 130 to 152 line)
                if (int.TryParse(KUTextBoxTempMin.Text, out int tempMinKU)) { }
                else
                {
                    MessageBox.Show("You should enter a number for min temperature for Kodiak", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KUTextBoxTempMin.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(KUTextBoxTempMax.Text, out int tempMaxKU))
                {
                    if (tempMaxKU < tempMinKU)
                    {
                        MessageBox.Show("The max temperatures should be higher than the min ones!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        KUTextBoxTempMax.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for max temperature for Kodiak", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KUTextBoxTempMax.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(KUTextBoxWind.Text, out int windCriteriaKU)) { }
                else
                {
                    MessageBox.Show("You should enter a number for the wind criteria for Kodiak", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KUTextBoxWind.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(KUTextBoxHum.Text, out int humCriteriaKU))
                {
                    if (humCriteriaKU > 100 || humCriteriaKU < 0)
                    {
                        MessageBox.Show("You should enter a number between 0 and 100 for the humidity criteria for Kodiak", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        KUTextBoxHum.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for the humidity criteria for Kodiak", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KUTextBoxHum.BackColor = Color.Yellow;
                    return;
                }






                //handling the possible errors for the Tanegashima spaceport and geting an individual message (from 160 to 182 line)
                if (int.TryParse(TJTextBoxTempMin.Text, out int tempMinTJ)) { }
                else
                {
                    MessageBox.Show("You should enter a number for min temperature for Tanegashima", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TJTextBoxTempMin.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(TJTextBoxTempMax.Text, out int tempMaxTJ))
                {
                    if (tempMaxTJ < tempMinTJ)
                    {
                        MessageBox.Show("The max temperatures should be higher than the min ones!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TJTextBoxTempMax.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for max temperature for Tanegashima", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TJTextBoxTempMax.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(TJTextBoxWind.Text, out int windCriteriaTJ)) { }
                else
                {
                    MessageBox.Show("You should enter a number for the wind criteria for Tanegashima", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TJTextBoxWind.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(TJTextBoxHum.Text, out int humCriteriaTJ))
                {
                    if (humCriteriaTJ > 100 || humCriteriaTJ < 0)
                    {
                        MessageBox.Show("You should enter a number between 0 and 100 for the humidity criteria for Tanegashima", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TJTextBoxHum.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for the humidity criteria for Tanegashima", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TJTextBoxHum.BackColor = Color.Yellow;
                    return;
                }






                //handling the possible errors for the Mahia spaceport and geting an individual message (from 190 to 212 line)
                if (int.TryParse(MNZTextBoxTempMin.Text, out int tempMinMNZ)) { }
                else
                {
                    MessageBox.Show("You should enter a number for min temperature for Mahia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MNZTextBoxTempMin.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(MNZTextBoxTempMax.Text, out int tempMaxMNZ))
                {
                    if (tempMaxMNZ < tempMinMNZ)
                    {
                        MessageBox.Show("The max temperatures should be higher than the min ones!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MNZTextBoxTempMax.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for max temperature for Mahia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MNZTextBoxTempMax.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(MNZTextBoxWind.Text, out int windCriteriaMNZ)) { }
                else
                {
                    MessageBox.Show("You should enter a number for the wind criteria for Mahia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MNZTextBoxWind.BackColor = Color.Yellow;
                    return;
                }
                if (int.TryParse(MNZTextBoxHum.Text, out int humCriteriaMNZ))
                {
                    if (humCriteriaMNZ > 100 || humCriteriaMNZ < 0)
                    {
                        MessageBox.Show("You should enter a number between 0 and 100 for the humidity criteria for Mahia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MNZTextBoxHum.BackColor = Color.Yellow;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You should enter a number for the humidity criteria for Mahia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MNZTextBoxHum.BackColor = Color.Yellow;
                    return;
                }


                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                //creating a list of all spaceports
                //it will be needed for finding the most suitable location and
                //for saving the results in a csv file
                List<Place> spacePorts = new List<Place>();

                //creating the first spaceport with individual criteria for the weather conditions
                Place spacePort1 = new Place()
                {
                    folderPath = fileNameKFG,
                    name = "Kourou (French Guyana)",
                    latitude = 5.1552,
                    tempMin = tempMinKFG,
                    tempMax = tempMaxKFG,
                    windCriteria = windCriteriaKFG,
                    humidityCriteria = humCriteriaKFG
                };
                spacePorts.Add(spacePort1);
                KourouTextBoxRes.Text = spacePort1.theBestDayForTheLocation;



                Place spacePort2 = new Place()
                {
                    folderPath = fileNameCCU,
                    name = "Cape Canaveral (USA)",
                    latitude = 28.3922,
                    tempMin = tempMinCCU,
                    tempMax = tempMaxCCU,
                    windCriteria = windCriteriaCCU,
                    humidityCriteria = humCriteriaCCU
                };
                spacePorts.Add(spacePort2);
                CapeCanTextBoxRes.Text = spacePort2.theBestDayForTheLocation;



                Place spacePort3 = new Place()
                {
                    folderPath = fileNameKU,
                    name = "Kodiak (USA)",
                    latitude = 57.79,
                    tempMin = tempMinKU,
                    tempMax = tempMaxKU,
                    windCriteria = windCriteriaKU,
                    humidityCriteria = humCriteriaKU
                };
                spacePorts.Add(spacePort3);
                KodiakTextBoxRes.Text = spacePort3.theBestDayForTheLocation;



                Place spacePort4 = new Place()
                {
                    folderPath = fileNameTJ,
                    name = "Tanegashima (Japan)",
                    latitude = 30.6590,
                    tempMin = tempMinTJ,
                    tempMax = tempMaxTJ,
                    windCriteria = windCriteriaTJ,
                    humidityCriteria = humCriteriaTJ
                };
                spacePorts.Add(spacePort4);
                TanegTextBoxRes.Text = spacePort4.theBestDayForTheLocation;



                Place spacePort5 = new Place()
                {
                    folderPath = fileNameMNZ,
                    name = "Mahia (New Zealand)",
                    latitude = -39.1536,
                    tempMin = tempMinMNZ,
                    tempMax = tempMaxMNZ,
                    windCriteria = windCriteriaMNZ,
                    humidityCriteria = humCriteriaMNZ
                };
                spacePorts.Add(spacePort5);
                MahiaTextBoxRes.Text = spacePort5.theBestDayForTheLocation;


                int index = FindTheClosestLocation(spacePorts);//geting the best case
                //entering the data in the textboxes
                textBoxTheBestLocation.Text = spacePorts[index].name;
                textBoxTheBestDate.Text = spacePorts[index].theBestDayForTheLocation;

                SaveFinalResults("BestDatesForLaunch.csv", spacePorts);//creating a csv file

                //handling errors with the emails and the password
                if (SenderTextBox.Text != "" && PasswordTextBox.Text != "" && ReceiverTextBox.Text != "")
                {
                    //sending an email
                    string body = $"The best combination of date and location is:\n{spacePorts[index].name}, day {spacePorts[index].theBestDayForTheLocation}";
                    string senderEmail = SenderTextBox.Text;
                    string password = PasswordTextBox.Text;
                    string receiverEmail = ReceiverTextBox.Text;
                    SendMail(senderEmail, password, receiverEmail, body, "BestDatesForLaunch.csv");
                }
                else
                {
                    MessageBox.Show("You haven't entered all needed data for sending email!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SenderTextBox.BackColor = Color.Yellow;
                    PasswordTextBox.BackColor = Color.Yellow;
                    ReceiverTextBox.BackColor = Color.Yellow;
                    return;
                }
            }
            else
            {
                MessageBox.Show("There is a wrongly entered or not entered folder path! Please, check again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //switching languages - unfortunately, it is not working so i had to comment it
        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch(comboBox1.SelectedIndex)
        //    {
        //        case 0:
        //            {
        //                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
        //                break;
        //            }
        //        case 1:
        //            {
        //                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-BE");
        //                break;
        //            }
        //    }
        //    this.Controls.Clear();
        //    InitializeComponent();
        //}
    }
}
