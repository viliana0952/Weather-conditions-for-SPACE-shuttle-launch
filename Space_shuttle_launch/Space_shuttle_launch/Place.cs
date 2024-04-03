using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Space_shuttle_launch
{
    class Place
    {
        public string name { get; set; }
        public double latitude { get; set; }
        public string folderPath { get; set; }
        public int tempMin { get; set; }
        public int tempMax { get; set; }
        public int windCriteria { get; set; }
        public int humidityCriteria { get; set; }

        //we don't need to have theBestDayForTheLocation as a Day because
        //to meet the expectations of the task and give the right answer we need only the date and not the other data about the day
        //after finding the most suitable day, if the user wanted to see the whole data about that very day, then theBestDayForTheLocation would be Day
        public string theBestDayForTheLocation
        {
            get
            {
                if(ResultDay()!=null)
                {
                    return ResultDay().number.ToString();
                }
                else
                    return "No suitable date";

            }
        }

        //a private method for reading all days
        //it is needed only inside the class
        private List<Day> _allDays ()
        {
            using (TextFieldParser parser = new TextFieldParser(folderPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                //string[] fieldsFirstRow = parser.ReadFields();
                string firstRow = parser.ReadLine(); 
                string[] fieldsFirstRowValues = firstRow.Split(';'); 
                Day[] allDaysArray = new Day[fieldsFirstRowValues.Length - 1];

                for (int i = 1; i < fieldsFirstRowValues.Length; i++)
                {
                    //allDaysArray[i - 1].number = int.Parse(fieldsFirstRow[i]);
                    int num = int.Parse(fieldsFirstRowValues[i]);
                    allDaysArray[i - 1] = new Day()
                    {
                        number = num
                    };
                }

                int counterRows = 2;
                while (!parser.EndOfData)
                {
                    string row = parser.ReadLine();
                    string[] fields = row.Split(';');

                    switch (counterRows)
                    {
                        case 2:
                            {
                                for (int i = 1; i < fields.Length; i++)
                                {
                                    allDaysArray[i - 1].temperature = int.Parse(fields[i]);
                                }
                                counterRows++;
                                break;
                            }
                        case 3:
                            {
                                for (int i = 1; i < fields.Length; i++)
                                {
                                    allDaysArray[i - 1].wind = int.Parse(fields[i]);
                                }
                                counterRows++;
                                break;
                            }
                        case 4:
                            {
                                for (int i = 1; i < fields.Length; i++)
                                {
                                    allDaysArray[i - 1].humidity = int.Parse(fields[i]);
                                }
                                counterRows++;
                                break;
                            }
                        case 5:
                            {
                                for (int i = 1; i < fields.Length; i++)
                                {
                                    allDaysArray[i - 1].precipitation = int.Parse(fields[i]);
                                }
                                counterRows++;
                                break;
                            }
                        case 6:
                            {
                                for (int i = 1; i < fields.Length; i++)
                                {
                                    if (fields[i].ToLower() == "no")
                                        allDaysArray[i - 1].lightning = false;
                                    else
                                        allDaysArray[i - 1].lightning = true;
                                }
                                counterRows++;
                                break;
                            }
                        case 7:
                            {
                                for (int i = 1; i < fields.Length; i++)
                                {
                                    allDaysArray[i - 1].clouds = fields[i];
                                }
                                counterRows++;
                                break;
                            }
                    }
                }
                return allDaysArray.ToList();
            }
        }
        
        //a private method for deciding which day is the most suitable one
        //the result will be saved in a public field
        private  Day  ResultDay ()
        {
            List<Day> days = _allDays();
            List<Day> daysToBeDeleted = new List<Day>();
            foreach(Day day in days)
            {
                if (day.temperature > tempMax || day.temperature < tempMin)
                {
                    daysToBeDeleted.Add(day);
                    continue;
                }
                if(day.wind>windCriteria)
                {
                    daysToBeDeleted.Add(day);
                    continue;
                }
                if(day.humidity>=humidityCriteria)
                {
                    daysToBeDeleted.Add(day);
                    continue;
                }
                if(day.precipitation!=0)
                {
                    daysToBeDeleted.Add(day);
                    continue;
                }
                if (day.lightning)
                {
                    daysToBeDeleted.Add(day);
                    continue;
                }
                if(day.clouds=="cumulus"||day.clouds=="nimbus")
                {
                    daysToBeDeleted.Add(day);
                    continue;
                }
            }

            days = days.Where(day => !daysToBeDeleted.Contains(day)).ToList();

            if (days.Count == 1)
                return days[0];
            else if (days.Count > 1) //in case there are more than one suitable days 
            {
                double[] averageDifference = new double[days.Count];
                int counter = 0;
                foreach (Day d in days)
                {
                    int diffW = 11 - d.wind;
                    int diffH = 55 - d.humidity;
                    averageDifference[counter] = (double)((diffW + diffH) / 2.0);
                    counter++;
                }

                var maxValInd = averageDifference
             .Select((value, index) => new { Value = value, Index = index })
             .Aggregate((max, next) => max.Value > next.Value ? max : next);

                return days[maxValInd.Index];
            }
            else
                return null;
        }
    }
}
