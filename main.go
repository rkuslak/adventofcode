package main

import (
	"fmt"
)

func main() {
	lines, err := LoadFile("2020-04-input.txt")
	if err != nil {
		panic(err)
	}

	validPassports := 0
	foundPassports := 0
	passportLines := []string{}
	readLinesCount := len(lines)

	for passportLinesIdx := 0; passportLinesIdx < readLinesCount; {
		for passportLinesIdx < readLinesCount && lines[passportLinesIdx] != "" {
			passportLines = append(passportLines, lines[passportLinesIdx])
			passportLinesIdx++
		}

		passportLinesIdx++
		foundPassports++
		if validatePassport(passportLines, true) {
			validPassports++
		}
		passportLines = []string{}
	}

	fmt.Printf("Found: %d, valid: %d", foundPassports, validPassports)
}
