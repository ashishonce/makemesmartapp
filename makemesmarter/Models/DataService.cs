using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

namespace makemesmarter.Models
{
	public static class DataService
	{
		public static List<DataPoint> GetRandomDataForNumericAxis(int count)
		{
			double y = 50;
			var _dataPoints = new List<DataPoint>();


			for (int i = 0; i < count; i++)
			{
               // var random = Math.Random();
				//y = y + (random.Next(0, 20) - 10);

				_dataPoints.Add(new DataPoint(i, y));
			}

			return _dataPoints;
		}

        public static List<DataPoint> GetCommentOverAllPie(DbSet<CommentThread> commentThreadList)
        {
            var statusBySortedList = commentThreadList.GroupBy(g => g.Status).Select(t => new { count = t.Count(), key = t.Key });
            double y = 50;
            string label = "";

            var _dataPoints = new List<DataPoint>();
            var count = 0;
            foreach (var g in statusBySortedList)
            {
                count += g.count;
            }

            foreach ( var item in statusBySortedList)
            {
                label = item.key;
                y = Math.Ceiling((double)item.count / (double)count * 100);
                _dataPoints.Add(new DataPoint(y, label));
            }

            return _dataPoints;
        }

        public static List<DataPoint> GetCommentByFileTypePie(DbSet<CommentThread> commentThreadList)
        {
            var fileTypeWhiteList = new List<string> { "cs", "ts", "js", "css", "chstml"};
            var statusBySortedList = commentThreadList.GroupBy(g => fileTypeWhiteList.Contains(g.FileType) ? g.FileType : "Random").Select(t => new { count = t.Count(), key = t.Key });

            double y = 50;
            string label = "";

            var _dataPoints = new List<DataPoint>();
            int count = 0;
            foreach(var g in statusBySortedList)
            {
                if(g.key == "Random")
                {
                    continue;
                }
                count += g.count;
            }

            foreach (var item in statusBySortedList)
            {
                if (item.key == "Random")
                {
                    continue;
                }
                y = Math.Ceiling((double)item.count/(double)count * 100);
                label = item.key;

                _dataPoints.Add(new DataPoint(y, label));
            }

            return _dataPoints;
        }

        public static List<DataPoint> GetCommentCategoryColumns(List<CommentThread> commentThreadList)
        {
            
            var statusBySortedList = commentThreadList.GroupBy(g => g.commentCategory).Select(t => new { count = t.Count(), key = t.Key });

            double y = 50;
            string label = "";

            var _dataPoints = new List<DataPoint>();
            var count = 0;
            foreach (var g in statusBySortedList)
            {
                if (g.key == "Random")
                {
                    continue;
                }
                count += g.count;
            }

            foreach (var item in statusBySortedList)
            {
                if (item.key == "Random")
                {
                    continue;
                }

                y = item.count;
                label = item.key;

                _dataPoints.Add(new DataPoint(y, label));
            }

            return _dataPoints;
        }

        public static List<DataPoint> GetRandomDataForCategoryAxis(int count)
		{
			double y = 50;
			DateTime dateTime = new DateTime(2006, 01, 1, 0, 0, 0);
			string label = "";

			var _dataPoints = new List<DataPoint>();


			for (int i = 0; i < count; i++)
			{
                var random = new Random();
				y = y + (random.Next(0, 20) - 10);
				label = dateTime.ToString("dd MMM");

				_dataPoints.Add(new DataPoint(y, label));
				dateTime = dateTime.AddDays(1);
			}

			return _dataPoints;
		}

		public static List<DataPoint> GetRandomDataForDateTimeAxis(int count)
		{
			double x = 0;
			double y = 50;

			var dateTime = new DateTime(2006, 01, 10, 0, 0, 0, DateTimeKind.Local);
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			var _dataPoints = new List<DataPoint>();


			for (int i = 0; i < count; i++)
			{
				//y = y + (random.Next(0, 20) - 10);

				x = dateTime.ToUniversalTime().Subtract(epoch).TotalMilliseconds;


				_dataPoints.Add(new DataPoint(x, y));
				dateTime = dateTime.AddDays(1);
				//dateTime = dateTime.AddHours(1);
			}

			return _dataPoints;
		}
	}
}