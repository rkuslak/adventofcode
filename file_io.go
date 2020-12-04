package main

import (
	"bufio"
	"os"
)

func LoadFile(path string) ([]string, error) {
	result := []string{}

	buf, err := os.Open(path)
	if err != nil {
		return []string{}, err
	}

	scanner := bufio.NewScanner(buf)
	for scanner.Scan() {
		result = append(result, scanner.Text())
	}

	return result, nil
}
