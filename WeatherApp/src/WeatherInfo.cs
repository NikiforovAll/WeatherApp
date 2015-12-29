using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;




namespace WeatherTracker
{
    public class WeatherInfo
    {
        public string City { get; set; }
        public string State { get; set; }
        public int Temperature { get; set; }
        public string WindState { get; set; }
        public string WindDirectionValue { get; set; }
        public int Humidity { get; set; }
        public double Pressure { get; set; }
        public DateTime Date { get; set; }
        public string IconId { get; set; }
        public double nHours;
        

        public WeatherInfo(double nHours = 1)
        {
            this.nHours = nHours;
           
        }
        /// <summary>
        /// asynchronous call , filling basic fields
        /// </summary>
        /// <returns></returns>
        /// 
       public async Task WeatherQuery()
        {
            
             await Task.Run(() =>
             {

                 string query = String.Format("http://api.openweathermap.org/data/2.5/weather?q=Odessa&mode=xml");
                 XmlDocument doc = new XmlDocument();
                 doc.Load(query);
                 XmlNode channel = doc.SelectSingleNode("current");
                 #region WI_FieldFetching
                 // city 
                 City = channel.SelectSingleNode("city").Attributes["name"].Value + ", ";
                 City += channel.SelectSingleNode("city").SelectSingleNode("country").InnerText;
                 //weather in  a few words 
                 State = channel.SelectSingleNode("weather").Attributes["value"].Value;
                 // temperature
                 string tmpStrTemp = channel.SelectSingleNode("temperature").Attributes["value"].Value.Replace('.', ',');
                 Temperature = (int)(Double.Parse(tmpStrTemp.Replace(',', '.')) - 273.15);
                 // Wind                 
                 WindState = channel.SelectSingleNode("wind").SelectSingleNode("speed").Attributes["name"].Value;
                 StringBuilder wStrB = new StringBuilder(channel.SelectSingleNode("wind").SelectSingleNode("direction").Attributes["name"].Value);
                 wStrB.Append(" - ")
                        .Append(
                     ( Double.Parse(channel.SelectSingleNode("wind").SelectSingleNode("direction").Attributes["value"].Value) / 10).ToString().Substring(0,4)
                        ).Append(" km/h");
                 WindDirectionValue = wStrB.ToString();
                 //atmosphere
                 Humidity = int.Parse(channel.SelectSingleNode("humidity").Attributes["value"].Value);// %
                 Pressure = int.Parse(channel.SelectSingleNode("pressure").Attributes["value"].Value);// hPa
                 // icon 
                 IconId = channel.SelectSingleNode("weather").Attributes["icon"].Value;
                 // Date of update
                 Date = DateTime.Now;
                 #endregion
             }
            );
        }

    }
}
