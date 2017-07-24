﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using makemesmarter.Models;

namespace makemesmarter.Helpers
{

    public static class MailContentCreator
    {
        public static string CreatePendingCommentResponse(List<CommentThread> commentList)
        {
            var content = "<html>";
            content += Getstyle();
            content += "<h2> Pending CR summary</h2>";
            content += "<table>";
            content += addHeader();
            foreach( var comment in commentList)
            {
                content += addRow(comment.PullRequestId.ToString(), comment.JoinedComments, comment.FilePath, "https://msasg.visualstudio.com/DefaultCollection/Bing_UX/Bing_UX%20Team/_git/snrcode/pullrequest/" + comment.PullRequestId + "#_a=overview");
            }
            
            content += "</table>";
            content += addDisclaimer();
            content += "</html>";

            return content;
        }

        private static string Getstyle()
        {
            var style = "<style> th,td{ padding:10px; border: 1px solid; }";
            style += "table{ border: 1px solid black; } </style> ";

            return style;
        }

        private static string addDisclaimer()
        {
            return "<p> Discalimer : The detials are subjective to changes; currently we don not support link on the mail</p>";
        }

        private static string addHeader()
        {
            return "<tr> <th> PR_ID</th> <th>Comment</th> <th> File</th></tr>";
        }

        private static string addRow(string PRId , string comment, string file , string url)
        {
            return string.Format("<tr> <td> <a href='{3}'> {0} </a> </td> <td> {1} </td> <td> {2} </td> </tr>", PRId, comment, file, url);
        }
    }
}
