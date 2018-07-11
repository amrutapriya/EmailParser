using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using EAGetMail;

namespace EmailParser
{
    public class EmailParser
    {
        public void Parse(string fileName)
        {
            // Parse the email using EAGetMail
            Mail email = new Mail("TryIt");
            email.Load(fileName, false);

            // Get IP Address and find the domain name
            string ipAddress = GetSendingIPAddress(email.Headers.ToString());
            if (!String.IsNullOrEmpty(ipAddress))
            {
                LookupHostName(ipAddress);
            }

            // Parse Uri's
            ParseUris(email.HtmlBody);
           
            Console.ReadLine();
        }

        /// <summary>
        /// Retrieves Received IP address using regualr expression
        /// </summary>
        /// <param name="headerText">Email header</param>
        /// <returns>IP address</returns>
        private string GetSendingIPAddress(string headerText)
        {
            // Filter based on Received from
            Regex regexForSendingIP = new Regex(@"Received: (.|\n  )*([^\d](\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}))+", RegexOptions.Multiline);
            var matches = regexForSendingIP.Matches(headerText);
            // Get the first match if match exists
            if (matches.Count > 0)
            {
                var group = matches[0].Groups[3];
                string ip = group.Captures[group.Captures.Count - 1].Value;
                Console.WriteLine("Sending IP Address: " + ip);
                return ip;
            }
            return null;
        }

        /// <summary>
        /// Reverse lookup based on IP and print domain name
        /// </summary>
        /// <param name="ip">IP address</param>
        private void LookupHostName(string ip)
        {
            try
            {
                // Revere lookup based on ip address
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                Console.WriteLine("Domain Name for IP Address: " + hostEntry.HostName);
            }
            catch(SocketException e)
            {
                Console.WriteLine("SocketException : " + e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine("FormatException : " + e.Message);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.Message);
            }
        }

        /// <summary>
        /// Parse uri's from the email body and print them
        /// </summary>
        /// <param name="body">Email Body</param>
        private void ParseUris(string body)
        {
             // Parse Uri Regex
            Regex regexForUri = new Regex(@"(https?|ftp|file)\://[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*");
            var findUriMatches = regexForUri.Matches(body);
            if (findUriMatches.Count > 0)
                Console.WriteLine("List of Uri's: ");

            // Print the list of uri's found
            foreach (var uriMatch in findUriMatches)
            {
                Console.WriteLine(uriMatch.ToString());
            }
        }
    }
}
