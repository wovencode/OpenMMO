//BY DX4D
using UnityEngine;

namespace OpenMMO.Chat
{
    [CreateAssetMenu(menuName = "OpenMMO/Chat/Profanity Filter")]
    public class ProfanityFilter : ScriptableObject
    {
#pragma warning disable CS0649
        [Header("PROFANITY FILTER")]
        [Tooltip("This will be shown instead of any words that are considered profanity.")]
        [SerializeField] internal string profanityMask = "****";

        [Tooltip("These words will be replaced in chat by the profanity mask.")]
        [SerializeField] internal string[] wordsToFilter;
#pragma warning restore CS0649

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
