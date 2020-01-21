using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KanColleManagementList
{
    class CSVInformation
    {

        /// <summary>
        /// 艦隊データCsvファイル格納場所
        /// </summary>
        static String KanColleCsvFilePath { get; set; }


        CSVInformation() 
        {
            KanColleCsvFilePath = Directory.GetCurrentDirectory()+ "\\Data\\KanColleData.csv";
        }

        /// <summary>
        /// DataTableの内容をCSVファイルに保存する
        /// </summary>
        /// <param name="dt">CSVに変換するDataTable</param>
        /// <param name="csvPath">保存先のCSVファイルのパス</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue。</param>
        public void ConvertDataTableToCsv(DataTable dt, string csvPath, bool writeHeader)
        {
            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");

            //書き込むファイルを開く
            System.IO.StreamWriter sr = new System.IO.StreamWriter(csvPath, false, enc);
            int colCount = dt.Columns.Count;
            int lastColIndex = colCount - 1;

            //ヘッダを書き込む
            if (writeHeader)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //ヘッダの取得
                    string field = dt.Columns[i].Caption;
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //レコードを書き込む
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //フィールドの取得
                    string field = row[i].ToString();
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //閉じる
            sr.Close();
        }

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }


        //dt:データを入れるDataTable
        //hasHeader:CSVの一行目がカラム名かどうか
        //fileName:ファイル名
        //separator:カラムを分けている文字(,など)
        //quote:カラムを囲んでいる文字("など)
        public void ReadCSV(DataTable dt, bool hasHeader, string fileName, string separator, bool quote)
        {
            //CSVを便利に読み込んでくれるTextFieldParserを使います。
            TextFieldParser parser = new TextFieldParser(fileName, Encoding.GetEncoding("UTF-8"));
            //これは可変長のフィールドでフィールドの区切りのマーカーが使われている場合です。
            //フィールドが固定長の場合は
            //parser.TextFieldType = FieldType.FixedWidth;
            parser.TextFieldType = FieldType.Delimited;
            //区切り文字を設定します。
            parser.SetDelimiters(separator);
            //クォーテーションがあるかどうか。
            //但しダブルクォーテーションにしか対応していません。シングルクォーテーションは認識しません。
            parser.HasFieldsEnclosedInQuotes = quote;
            string[] data;
            //ここのif文では、DataTableに必要なカラムを追加するために最初に1行だけ読み込んでいます。
            //データがあるか確認します。
            if (!parser.EndOfData)
            {
                //CSVファイルから1行読み取ります。
                data = parser.ReadFields();
                //カラムの数を取得します。
                int cols = data.Length;
                if (hasHeader)
                {
                    for (int i = 0; i < cols; i++)
                    {
                        dt.Columns.Add(new DataColumn(data[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < cols; i++)
                    {
                        //カラム名にダミーを設定します。
                        dt.Columns.Add(new DataColumn());
                    }
                    //DataTableに追加するための新規行を取得します。
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < cols; i++)
                    {
                        //カラムの数だけデータをうつします。
                        row[i] = data[i];
                    }
                    //DataTableに追加します。
                    dt.Rows.Add(row);
                }
            }
            //ここのループがCSVを読み込むメインの処理です。
            //内容は先ほどとほとんど一緒です。
            while (!parser.EndOfData)
            {
                data = parser.ReadFields();
                DataRow row = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row[i] = data[i];
                }
                dt.Rows.Add(row);
            }
        }

        /// <summary>
        /// 実行場所\List\KanColleList.csvが存在するか確認する
        /// </summary>
        /// <returns>存在する場合はture,存在しない場合はfalse</returns>
        public Boolean CheckCSVFile() {
            //Csvファイルカレントディレクトリパスの取得
            String CSVFilePath = (System.Environment.CurrentDirectory)+ "\\List\\KanColleList.csv";
            if (System.IO.File.Exists(CSVFilePath))
            {
                return true;
            }
            else {
                return false;
            }
 
        }

    }
}
