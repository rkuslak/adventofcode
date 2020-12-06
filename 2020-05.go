package main

import (
	"fmt"
	"sort"
)

func getRowFromTicket(ticket string) int64 {
	var row int64 = 0

	ticketRow := ticket[0:7]

	for _, incomingBit := range ticketRow {
		row = row << 1
		if incomingBit == 'B' {
			row = row ^ 0x1
		}
	}

	return row
}

func getColumnFromTicket(ticket string) int64 {
	var column int64 = 0

	ticketColumn := ticket[7:]

	for _, incomingBit := range ticketColumn {
		column = column << 1
		if incomingBit == 'R' {
			column = column ^ 0x1
		}
	}

	return column
}

func run202005() {
	lines, err := LoadFile("2020-05-input.txt")
	if err != nil {
		panic(err)
	}

	seatIds := [](int){}
	maxBoardingPassId := 0

	for _, line := range lines {
		if len(line) != 10 {
			fmt.Printf("Skipping line: \"%s\"\n", line)
			continue
		}
		row := getRowFromTicket(line)
		col := getColumnFromTicket(line)
		seatId := int(row*8 + col)
		seatIds = append(seatIds, seatId)
		if maxBoardingPassId < seatId {
			maxBoardingPassId = seatId
		}
	}

	sort.Ints(seatIds)
	fmt.Printf("Max bording pass id: %d\n", maxBoardingPassId)
	seatsToCompare := len(seatIds) - 1
	for idx := 0; idx < seatsToCompare; idx++ {
		if seatIds[idx+1]-2 == seatIds[idx] {
			fmt.Printf("Your seat: %d\n", seatIds[idx]+1)
		}
	}
}
