/*
データテーブルを処理する
 */
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KanColleManagementList
{
    class DataInfo
    {
        /// <summary>
        /// CSVファイルの操作用クラスの呼び出し
        /// </summary>
        CSVInformation CSVinf = new CSVInformation();

        /// <summary>
        /// 艦隊一覧を格納する変数
        /// </summary>
        //private DataTable KanColleData = new DataTable();
        public DataTable KanColleData { get; set; }
        
        /// <summary>
        /// フォルダにあるcsv格納する。
        /// </summary>
        public DataTable oldKanColleData { get; set; }

        /// <summary>
        /// 読み込んだcsvを格納する
        /// </summary>
        private DataTable inputKanColleData { get; set; }

        /// <summary>
        /// 起動時のコンストラクタ。
        /// 起動時にDataフォルダにあるcsvを読み込む
        /// </summary>
        public DataInfo()
        {
            //データテーブルのインスタンス化
            KanColleData = new DataTable();
            oldKanColleData = new DataTable();
            inputKanColleData = new DataTable();

            //ファイルがあればデータを読み込む
            if (File.Exists(CSVinf.KanColleCsvFilePath))
            {
                CSVinf.ReadCSV(KanColleData, true, CSVinf.KanColleCsvFilePath, ",", false);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable UpdateViewDataTable() 
        {
            //現在のデータをoldデータとしてコピーする.
            oldKanColleData = KanColleData.Copy();
            
            //ダイアログインスタンス作成
            var dialog = new OpenFileDialog();
            //ファイルの種類の設定
            dialog.Filter = "csvファイル (*.csv)|*.csv|全てのファイル (*.*)|*.*";
            //ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                //読み込んだcsvをデータテーブルに追加
                CSVinf.ReadCSV(inputKanColleData, true, dialog.FileName, ",", false);
            }
            else 
            {
                //キャンセルされた場合はデータはそのまま。
                return KanColleData;
            }



            //現在のcsvファイルをoldファイルとして置き換える
            ReplaceFiles(CSVinf.KanColleCsvFilePath, CSVinf.oldKanColleCsvFilePath);

            return KanColleData;
        }

        /// <summary>
        /// ファイルの置き換えを行う。
        /// 置き換えるファイルがない場合はスキップされる。
        /// 同名のファイルがある場合は上書きされます。
        /// </summary>
        /// <param name="oldFoleName">置き換えるファイル名</param>
        /// <param name="NewFileName">置き換え後のファイル名</param>
        public void ReplaceFiles( String oldFoleName,String NewFileName ) 
        {
            if (File.Exists(oldFoleName)) 
            {
                File.Copy(oldFoleName, NewFileName,true);
            }
        }

        private String BattleShipMapping(String BattleShipType)
        {
            String Type = "";
            switch ( BattleShipType ) 
            {
                case "戦艦":
                case "巡洋戦艦":
                case "航空戦艦":
                case "超弩級戦艦":
                    Type = "戦艦";
                break;
                case "空母":
                case "装甲空母":
                case "軽空母":
                    Type = "空母";
                break;
                case "重巡洋艦":
                case "航空巡洋艦":
                    Type = "重巡洋艦";
                break;
                case "軽巡洋艦":
                case "重雷装巡洋艦":
                case "練習巡洋艦":
                case "防空巡洋艦":
                    Type = "軽巡洋艦";
                break;
                case "駆逐艦":
                    Type = "駆逐艦";
                break;
                case "海防艦":
                    Type = "海防艦";
                break;
                case "潜水艦":
                case "潜水母艦":
                    Type = "潜水艦";
                break;
                default:
                    Type = "その他";
                break;
            }
            return Type;
        }

    }
}
