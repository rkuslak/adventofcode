package main

import (
	"fmt"
)

func run202006() {
	lines, err := LoadFile("2020-06-input.txt")
	if err != nil {
		panic(err)
	}

	totalAnswers := 0
	matchedAnswers := 0
	parsedLines := 0
	answerKey := make(map[rune]int)

	for _, line := range lines {
		if line == "" {
			totalAnswers += len(answerKey)
			for _, value := range answerKey {
				if value == parsedLines {
					matchedAnswers++
				}
			}
			answerKey = make(map[rune]int)
			parsedLines = 0
			continue
		}

		// dedup line
		for _, lineRune := range line {
			currentValue := answerKey[lineRune]
			currentValue++

			answerKey[lineRune] = currentValue
		}
		parsedLines++
	}

	// Last iteration should have fresh map if no new values anyway:
	totalAnswers += len(answerKey)
	for _, value := range answerKey {
		if value == parsedLines {
			matchedAnswers++
		}
	}

	fmt.Printf("total: %d, matched: %d", totalAnswers, matchedAnswers)
}
