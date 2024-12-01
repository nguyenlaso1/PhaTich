using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public class NGUIUtils : MonoBehaviour {
	
	private static Regex arabicRegex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
	private static char[] punctuation = "!\"#%&\'()*,-./:;?@[]\\_{}+".ToCharArray();
	
	private static string[] tempWords;
	private static int tempCount;
	
	public static bool ContainArabicCharacters(string text) {
    return arabicRegex.IsMatch(text);
	}
	
	public static string ReverseWords(string sentence) {
		string final = "";
		string[] se = sentence.Split('\n');
		string[] words;
		for (int i = 0; i < se.Length; i++) {
			words = se[i].Split(' ');
			Array.Reverse(words);
			if (i < se.Length - 1) {
				final += string.Join(" ", words) + "\n";
			} else {
				final += string.Join(" ", words);
			}
		}
		// string[] words = sentence.Split(' ');
		return final;
	}
	
	public static string RevertSpecialWordArabic(string text) {
		tempWords = text.Split(' ');
		text = string.Empty;
		string cacheWord;
		int stringLength = tempWords.Length;
		for (tempCount = 0; tempCount < stringLength; tempCount++) {
			cacheWord = tempWords[tempCount];
			
			if (cacheWord.Length > 1 && ContainPunctuation(cacheWord) && !ContainArabicCharacters(cacheWord)) {
				bool swapTop = false;
				bool swapBottom = false;
				string topChars = "";
				string bottomChars = "";
				int indexTop = 0;
				int indexBottom = cacheWord.Length;
				if (char.IsPunctuation(cacheWord[0])) {
					swapTop = true;
					for (int i = 0; i < cacheWord.Length; i++) {
						if (char.IsPunctuation(cacheWord[i])) {
							topChars += cacheWord[i];							
						} else {
							indexTop = i;
							break;
						}
					}
				}
				if (char.IsPunctuation(cacheWord[cacheWord.Length - 1])) {
					swapBottom = true;
					for (int i = cacheWord.Length - 1; i >= 0; i--) {
						if (char.IsPunctuation(cacheWord[i])) {
							bottomChars = cacheWord[i] + bottomChars;
						} else {
							indexBottom = i + 1;
							break;
						}
					}
				}				
				
				tempWords[tempCount] = "";
				if (swapBottom) {
					tempWords[tempCount] += bottomChars;
				}
				tempWords[tempCount] += cacheWord.Substring(indexTop, indexBottom - indexTop);
				// if (indexBottom < cacheWord.Length) {
				// 	tempWords[tempCount] += cacheWord.Substring(indexTop, indexBottom);
				// } else {
				// 	tempWords[tempCount] += cacheWord.Substring(indexTop, cacheWord.Length - 1);
				// }
				if (swapTop) {
					tempWords[tempCount] += topChars;
				}				
			}
			text += tempWords[tempCount];
			if (tempCount < stringLength - 1) {
				text += " ";
			}
		}
		return text;
	}
	
	public static bool ContainPunctuation(string word) {
		return word.IndexOfAny(punctuation) != -1;
		// return specialCharRegex.IsMatch(word);
	}
}
