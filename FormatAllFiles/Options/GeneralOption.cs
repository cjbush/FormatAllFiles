﻿using System;
using System.ComponentModel;
using System.IO;
using FormatAllFiles.Text;

namespace FormatAllFiles.Options
{
    /// <summary>
    /// 全般設定のオプションです。
    /// </summary>
    public class GeneralOption
    {
        /// <summary>
        /// ドキュメントをフォーマットするコマンドです。
        /// </summary>
        private const string FORMAT_DOCUMENT_COMMAND = "Edit.FormatDocument";

        /// <summary>
        /// 各ファイルに実行するコマンドを取得または設定します。
        /// </summary>
        [Category("Command")]
        [DisplayName("Execution Command")]
        [Description("A command to execute each files.")]
        public string Command { get; set; }

        /// <summary>
        /// T4で作成されたファイルを対象から除外するかどうかを取得または設定します。
        /// </summary>
        [Category("Target File")]
        [DisplayName("Exclude Generated T4")]
        [Description("Exclude target files generated by T4 text template.")]
        public bool ExcludeGeneratedT4 { get; set; }

        /// <summary>
        /// 対象とするファイルを一致させるワイルドカードのパターンです。
        /// </summary>
        [Category("Target File")]
        [DisplayName("Inclusion Pattern")]
        [Description("This is a pattern to search inclusion files. You can use the wild card \"*\" and \"?\" like \"*.cs\". When this is empty, all files apply.")]
        public string InclusionFilePattern { get; set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public GeneralOption()
        {
            Command = FORMAT_DOCUMENT_COMMAND;
            InclusionFilePattern = "*.*";
            ExcludeGeneratedT4 = true;
        }

        /// <summary>
        /// 対象ファイルを名前で絞り込むフィルターを作成します。
        /// </summary>
        /// <returns>ファイル名で絞り込む処理</returns>
        public Func<string, bool> CreateFileFilter()
        {
            if (string.IsNullOrWhiteSpace(InclusionFilePattern))
            {
                return name => true;
            }
            else
            {
                var wildCard = new WildCard(InclusionFilePattern);
                return wildCard.IsMatch;
            }
        }

        /// <summary>
        /// 対象ファイルを階層のパスで絞り込むフィルターを作成します。
        /// </summary>
        /// <returns>階層のパスで絞り込む処理</returns>
        public Func<string, bool> CreateHierarchyFilter()
        {
            if (ExcludeGeneratedT4)
            {
                return path => Path.GetExtension(path) != ".tt";
            }
            else
            {
                return path => true;
            }
        }
    }
}
