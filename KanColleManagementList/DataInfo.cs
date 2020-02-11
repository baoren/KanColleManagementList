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
        /// 開くボタンが押されたときに呼び出される
        /// </summary>
        public void RoadDataTable() 
        {
            //CSVファイルを読み込んだときのみファイルを置き換える
            if (ReadInputDataTable())
            {
                //構造のみをのこしてデータを削除
                KanColleData.Clear();
                //現在のcsvファイルをoldファイルとして置き換える
                ReplaceFiles(CSVinf.KanColleCsvFilePath, CSVinf.oldKanColleCsvFilePath);
                //データの更新
                AssembleDataTable();
                //インプットしたデータを初期化する
                inputKanColleData.Reset();
                oldKanColleData.Reset();
            }
        }

        /// <summary>
        /// 本体データ組み立て用関数
        /// </summary>
        public void AssembleDataTable() 
        {
            //ID検索
            for ( int row=0; row<inputKanColleData.Rows.Count; row++ ) 
            {
                DataRow dataRow = KanColleData.NewRow();
                //旧データに読み込んだデータのIDがあるかチェック
                DataRow[] Check = oldKanColleData.Select("ID = "+ inputKanColleData.Rows[row]["ID"]);

                //IDは変わらないためそのまま入れる
                KanColleData.Rows[row]["ID"] = inputKanColleData.Rows[row]["ID"].ToString();
                //艦名は読み込んだデータの物を正とする
                KanColleData.Rows[row]["艦名"] = inputKanColleData.Rows[row]["艦名"].ToString();
                //Levelは読み込んだデータを入れる
                KanColleData.Rows[row]["Lv"] = int.Parse(inputKanColleData.Rows[row]["Lv"].ToString());
                //艦種は読み込んだデータを入れる
                KanColleData.Rows[row]["艦種"] = inputKanColleData.Rows[row]["艦種"].ToString();
                //艦種の型変換
                KanColleData.Rows[row]["艦種型"] = BattleShipMapping(KanColleData.Rows[row]["艦種"].ToString());
                //一致するデータがあった場合
                if (Check.Length == 1)
                {
                    //前回のレベルを入れる
                    KanColleData.Rows[row]["前回Lv"] = int.Parse(Check[0]["Lv"].ToString());
                    //前回からの変動値を計算する。
                    //他にいい方法があれば変える予定
                    KanColleData.Rows[row]["変動値"] = int.Parse(inputKanColleData.Rows[row]["Lv"].ToString()) - int.Parse(Check[0]["Lv"].ToString());
                }
                else 
                {
                    //一致しないIDには0を入れる
                    KanColleData.Rows[row]["前回Lv"] = 0;
                    KanColleData.Rows[row]["変動値"] = 0;
                }
            }
        }

        /// <summary>
        /// CSVを読み込んでデータテーブルに格納する。
        /// </summary>
        /// <returns>ファイルの読み込み有無を返す</returns>
        public Boolean ReadInputDataTable() 
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
                return true;
            }
            else { return false; }
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

        public String BattleShipMapping(String BattleShipType)
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
