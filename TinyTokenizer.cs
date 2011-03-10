// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Defines the TinyTokenizer type.
//   Provides a single method to split text into a list of individual, unique tokens across word boundaries, with unwanted tokens discarded.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TinyTokenizer
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the TinyTokenizer type.
    /// Provides a single method to split text into a list of individual, unique tokens across word boundaries, with unwanted tokens discarded.
    /// </summary>
    public class TinyTokenizer
    {
        /// <summary>
        /// Tokens that should be discarded by the tokenizer
        /// </summary>
        private readonly List<string> ignoreTokens = new List<string> 
        {
            string.Empty, ".", ",", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "?", "[", "]", "{", "}", "\\", "|", "<", "\"", ":", ";", "'", "~", "`",
            "a", "all", "am", "an", "and", "any", "are", "as", "at", "be", "but", "can", "did", "do", "does", "for", "from", "had", "has", "have", "here", "how", "i", "if", "in", "is", "it", "no", "not", "of", "on", "or", "so", "that", "the", "then", "there", "this", "to", "too", "up", "use", "what", "when", "where", "who", "why", "you", "i", "br",
            "は", "を", "の", "が", "で", "に", "も", "し", "から", "って", "これ", "へ", "と", "より", "や", "やら", "なり", "か", "だの", "かしら", "な", "とも", "ぞ", "わ", "さ", "よ", "ね", "ばかり", "まで", "だけ", "ほど", "くらい", "など", "やら", "こそ", "でも", "しか", "さえ", "だに", "けれども", "こと", "よう", "」", 
            "て", "ず", "た", "れ", 
            "！", "＠", "＃", "＄", "％", "＾", "＆", "＊", "＿", "＋", "＝", "－", "％", "（", "）", "、", "。", "・", "【", "　", "｛", "｝", "：", "；", "”", "’", "＜", "＞", "？", "｜", "￥", "～",
        };

        /// <summary>
        /// Characters that should be discarded by the tokenizer
        /// </summary>
        private readonly List<char> ignoreChars = new List<char> 
        {
            '.', ',', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '?', '[', ']', '{', '}', '\\', '|', '<', '\'', ':', ';', '\'', '~', '`'
        };

        /// <summary>
        /// Gets or sets the TinySegmenter.
        /// </summary>
        private readonly TinySegmenter segmenter = new TinySegmenter();

        /// <summary>
        /// Splits the given input text into a list of individual, unique tokens across word boundaries, with unwanted tokens discarded.
        /// </summary>
        /// <param name="input">The text to tokenize.</param>
        /// <returns>A list of tokens corresponding to the given input text.</returns>
        public List<string> Tokenize(string input)
        {
            // Segment the input text
            var segments = this.segmenter.Segment(input);

            // Tokenize all segments
            var tokens = segments.ConvertAll(segment => this.TokenizeSingle(segment));

            // Remove null values
            tokens.RemoveAll(token => token == null);

            // De-dupe
            tokens = tokens.Distinct().ToList();

            return tokens;
        }

        /// <summary>
        /// Tokenizes a single segmented string
        /// </summary>
        /// <param name="segment">The segmented string</param>
        /// <returns>A trimmed, lowercase version of the segmented string, or null if not a valid token</returns>
        private string TokenizeSingle(string segment)
        {
            // Trim any trailing and leading space and Convert to lowercase
            var token = segment.Trim().ToLower();

            // Strip any remaining punctuation
            token = new string(token.ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray());

            // Strip special characters
            token = new string(token.ToCharArray().Where(c => !this.ignoreChars.Contains(c)).ToArray());

            // Don't allow ignored tokens and single letter strings
            token = (token.Length > 1 && !this.ignoreTokens.Contains(token)) ? token : null;

            return token;
        }
    }
}
