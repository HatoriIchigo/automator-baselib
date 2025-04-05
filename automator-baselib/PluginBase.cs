using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace automator_baselib
{
    abstract public class PluginBase
    {
        public string identifier { set; get; } = "";
        public Dictionary<string, string> properties { set; get; } = new Dictionary<string, string>();
        public Dictionary<string, ActionItem> actionItems = new Dictionary<string, ActionItem>();

        public virtual IEnumerable<string> Init()
        {
            // identifierの制限
            if (identifier.Length != 3)
            {
                yield return new Output("E-cmn-9001", "Identifierが3文字になっていません。", "", "").toString();
                yield break;
            }
            if (identifier == "cmn")
            {
                yield return new Output("E-cmn-9002", "Identifierにcmnは使用できません。", "", "").toString();
                yield break;
            }

            // 共通設定ファイル読み込み
            string err = ReadConfigFile(Path.Combine(Directory.GetCurrentDirectory(), "config", "common.ini"));
            if (err != "")
            {
                yield return err;
                yield break;
            }

            // 個別ファイル読み込み
            err = ReadConfigFile(Path.Combine(Directory.GetCurrentDirectory(), "config", identifier + ".ini"));
            if (err != "")
            {
                yield return err;
                yield break;
            }

            yield return "";
        }

        public abstract IEnumerable<string> CheckCmd(string cmd);
        public virtual IEnumerable<string> Action(string input, string cmd)
        {
            foreach (var item in this.actionItems)
            {
                if (cmd.Split('[')[0] == item.Key)
                {
                    foreach (string log in this.actionItems[item.Key].Action(input, cmd))
                    {
                        yield return log;
                        yield break;
                    }
                }
            }
            yield return new Output("E-cmn-9007", "コマンドが見つかりませんでした。[cmd: " + cmd + "]", "", "").toString();
        }

        private string ReadConfigFile(string configPath)
        {
            if (!File.Exists(configPath))
            {
                return new Output("E-cmn-9003", "設定ファイルが見つかりません。[configPath:  " + configPath + "]", "", "").toString();
            }

            foreach (string line in File.ReadAllLines(configPath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#")) continue;
                string[] d = line.Split(new char[] { '=' });
                if (d.Length == 2)
                {
                    this.properties[d[0].Trim()] = d[1].Trim();
                }
                else
                {
                    return new Output("F-cmn-9004", "設定ファイルに異常な値が見つかりました。[configFilePath: " + configPath + " line:  " + line + "]", "", "").toString();
                }
            }
            return "" ;
        }

        public abstract IEnumerable<string> Reset();

        public string GetStructure(string cmd)
        {
            foreach (var item in this.actionItems)
            {
                if (cmd == item.Key)
                {
                    return item.Value.getStructure();
                }
            }
            return new Output("E-cmn-9008", "コマンドが見つかりませんでした。[cmd: " + cmd + "]", "", "").toString();
        }

        public virtual string ParseInput(Dictionary<string, string> dict, string input)
        {
            
            return "";
        }
        public virtual string ParseCmd(Dictionary<string, string> dict, string input, string cmd)
        {
            string s = input.Substring(cmd.Length);
            if (s == "") { return ""; }
            if (Regex.IsMatch(s, @"^\[.*\]$"))
            {
                s = s.Substring(1, s.Length - 2);
                foreach (string key_value in s.Split(new char[] { '&' }))
                {
                    string[] x = key_value.Split(new char[] { '=' });
                    if (x.Length != 2) { return new Output("E-cmn-9006", "入力値に異常な値が見つかりました。[入力値: " + s + "]", "", "").toString(); }
                    dict.Add(x[0].Trim(), x[1].Trim());
                }
                return "";
            }
            else
            {
                return new Output("E-cmn-9005", "入力値に異常な値が見つかりました。[入力値: " + s + "]", "", "").toString();
            }
        }
    }
}
