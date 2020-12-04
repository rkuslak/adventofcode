package main

import (
	"fmt"
	"strconv"
)

func findJoin(lines []string) (int, int, error) {
	intValues := []int{}

	for _, line := range lines {
		intValue, err := strconv.Atoi(line)
		if err != nil {
			panic(err)
		}
		intValues = append(intValues, intValue)
	}
	linesCount := len(intValues)

	for leftIdx := 0; leftIdx < linesCount; leftIdx++ {
		leftValue := intValues[leftIdx]
		for rightIdx := leftIdx + 1; rightIdx < linesCount; rightIdx++ {
			rightValue := intValues[rightIdx]
			if rightValue+leftValue == 2020 {
				return leftValue, rightValue, nil
			}
		}
	}

	return -1, -1, fmt.Errorf("No match found")
}

func findDeepJoin(lines []string) (int, int, int, error) {
	intValues := []int{}

	for _, line := range lines {
		intValue, err := strconv.Atoi(line)
		if err != nil {
			panic(err)
		}
		intValues = append(intValues, intValue)
	}

	linesCount := len(intValues)

	for leftIdx := 0; leftIdx < linesCount; leftIdx++ {
		leftValue := intValues[leftIdx]

		for centerIdx := leftIdx + 1; centerIdx < linesCount; centerIdx++ {
			centerValue := intValues[centerIdx]

			for rightIdx := centerIdx + 1; rightIdx < linesCount; rightIdx++ {
				rightValue := intValues[rightIdx]

				if rightValue+leftValue+centerValue == 2020 {
					return leftValue, centerValue, rightValue, nil
				}

			}

		}

	}

	return -1, -1, -1, fmt.Errorf("No match found")
}
