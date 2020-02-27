//IMPROVEMENTS BY DX4D
using UnityEngine;

namespace OpenMMO.Chat
{
    [CreateAssetMenu(menuName = "OpenMMO/Chat/Profanity Filter")]
    public class ProfanityFilter : ScriptableObject
    {
        [Header("PROFANITY FILTER")]
        [Tooltip("This will be shown instead of any words that are considered profanity.")]
        [SerializeField] string profanityMask = "****";

        [Tooltip("These words will be replaced in chat by the profanity mask.")]
        [SerializeField] string[] wordsToFilter;

        public string FilterText(string text)
        {
            foreach (string word in wordsToFilter)
            {
                text = text.ReplaceIgnoreCase(word, profanityMask);
            }

            return text;
        }
    }
}
