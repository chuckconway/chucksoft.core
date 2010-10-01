using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Chucksoft.Core.Web.Validation
{
    public class Profanity
    {
        private const int INIT_MAX = 17;
        private static readonly object _lock = new object();
        private static XmlDocument doc;
        private static Regex regex;
        private string[,] arrCharRep = new string[INIT_MAX,2];
        private int count;
        private int max = INIT_MAX;

        private string regExpression = "";


        /// <summary>
        /// Initializes a new instance of the <see cref="Profanity"/> class.
        /// </summary>
        public Profanity()
        {
            if (doc == null || regex == null)
            {
                lock (_lock)
                {
                    if (doc == null)
                    {
                        doc = new XmlDocument();

                        string profanityXml = ResourceFileHelper.ConvertStreamResourceToUnicodeString(typeof(Profanity),
                                                                                           "Conway.Web.UI.Security.Profanity.xml");
                        profanityXml = profanityXml.Trim();
                        doc.LoadXml(profanityXml);

                        LoadDoc();
                    }

                    if (regex == null)
                    {
                        regex = new Regex(regExpression,
                                          RegexOptions.IgnoreCase | RegexOptions.Compiled |
                                          RegexOptions.IgnorePatternWhitespace);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is offensive helper] [the specified phrase].
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns>
        /// 	<c>true</c> if [is offensive helper] [the specified phrase]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsOffensiveHelper(string phrase)
        {
            bool isOffensive = false;

            //does it match a bad word?
            if (regex.IsMatch(phrase))
            {
                isOffensive = true;
            }

            //if a badword is not found, replace some characters and run it again.
            if (!isOffensive)
            {
                phrase = ReplaceChar(phrase);

                if (regex.IsMatch(phrase))
                {
                    isOffensive = true;
                }
            }

            return isOffensive;
        }

        /// <summary>
        /// Determines whether the specified valid string is offensive.
        /// </summary>
        /// <param name="validString">The valid string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified valid string is offensive; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOffensive(string validString)
        {
            var instance = new Profanity();
            return instance.IsOffensiveHelper(validString);
        }

        /// <summary>
        /// Loads the doc.
        /// </summary>
        private void LoadDoc()
        {
            XmlElement root = doc.DocumentElement;
            if (root != null)
            {
                XmlNodeList _objNodeList = root.SelectNodes("words/word");

                if (_objNodeList != null)
                {
                    for (int i = 0; i < _objNodeList.Count; i++)
                    {
                        //true for substring
                        //trur for repition

                        string word = _objNodeList[i].Attributes["id"].InnerText;
                        bool isSubString = _objNodeList[i].Attributes["match-type"].InnerText.Equals("substring");
                        add(word, isSubString);
                    }
                }
            }

            ReadToCharArray(doc);


            XmlNodeList nodeList = doc.SelectNodes(string.Format("//root/words[contains(@type,'{0}')]", GetCulture()));
            XmlNodeList objNodeList;

            if (nodeList != null)
            {
                for (int m = 0; m < nodeList.Count; m++)
                {
                    objNodeList = nodeList.Item(m).SelectNodes("word");

                    if (objNodeList != null)
                    {
                        for (int i = 0; i < objNodeList.Count; i++)
                        {
                            add(objNodeList[i].Attributes["id"].InnerText,
                                objNodeList[i].Attributes["match-type"].InnerText.Equals("substring"));
                        }
                    }
                }
            }

            ReadToCharArray(doc);
        }

        /// <summary>
        /// Reads to char array.
        /// </summary>
        /// <param name="document">The document.</param>
        private void ReadToCharArray(XmlDocument document)
        {
            XmlDocument xdoc = document;

            XmlNodeList childNodeList;
            XmlElement root = xdoc.DocumentElement;

            if (root != null)
            {
                XmlNodeList objNodeList = root.SelectNodes("options/equivalences/char");

                if (objNodeList != null)
                {
                    for (int i = 0; i < objNodeList.Count; i++)
                    {
                        childNodeList = objNodeList[i].ChildNodes;
                        string key = objNodeList[i].Attributes["id"].InnerText;

                        for (int j = 0; j < childNodeList.Count; j++)
                        {
                            add(key, childNodeList[j].InnerText);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the simple expression.
        /// </summary>
        /// <param name="word">The word.</param>
        private void addSimpleExpression(string word)
        {
            if (regExpression.Length <= 0)
            {
                regExpression = word;
            }
            else
            {
                regExpression = regExpression + "|" + word;
            }
        }

        /// <summary>
        /// Inserts the blank space.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        private static string InsertBlankSpace(string word)
        {
            int j = word.Length;
            const string StartandEnd = "\\b";

            var final = new StringBuilder();
            for (int i = 0; i <= j - 1; i++)
            {
                final.Append(word.Substring(i, 1)).Append("\\s*"); //'after every letter add \s*
            }

            return StartandEnd + final.ToString().Substring(0, final.Length - 3) + StartandEnd;
                //'Cut out last separator (h-e-l-l-o- -> h-e-l-l-o)
        }

        /// <summary>
        /// Adds the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="isSubstring">if set to <c>true</c> [is substring].</param>
        private void add(string word, bool isSubstring)
        {
            string strblnk = InsertBlankSpace(word);

            addSimpleExpression(strblnk);

            if (isSubstring)
            {
                addSimpleExpression("(" + word + ")");
            }
            else
            {
                addSimpleExpression("(\\b" + word + "\\b)");
            }
        }

        /// <summary>
        /// Replaces the char.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        private string ReplaceChar(string str)
        {
            for (int i = 0; i < count; i++)
            {
                if (arrCharRep[i, 0] != null)
                {
                    str = str.Replace(arrCharRep[i, 1], arrCharRep[i, 0]);
                }
            }
            return str;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void add(string key, string value)
        {
            if (count < max - 1)
            {
                arrCharRep[count, 0] = key;
                arrCharRep[count, 1] = value;
                count++;
            }
            else
            {
                max = max*2;
                var temp = new string[max,2];
                Array.Copy(arrCharRep, temp, arrCharRep.Length);
                arrCharRep = temp;
                add(key, value);
            }
        }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <returns></returns>
        private static string GetCulture()
        {
            // *** For code reusability.  A webapp can only support one market at a time.
            //System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("/web.config") as System.Configuration.Configuration;
            //ConfigurationSection cs = config.GetSection("system.web/globalization");
            //string culture = ((System.Web.Configuration.GlobalizationSection)cs).Culture;
            //if (!string.Equals("es-MX", culture))
            //{
            //    culture = "en-US";
            //}

            //return string.IsNullOrEmpty(culture) ? "en-US" : culture;


            // for now return en-US            
            return "en-US";
        }
    }
}