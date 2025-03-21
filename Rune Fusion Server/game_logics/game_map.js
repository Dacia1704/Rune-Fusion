import getRandomInt from "../utils/random.js";

let MapData;

const rate_protected = 20;
export default function generateRuneMap(mapData) {
    const map = [];
    MapData = mapData;
    for (let rowIndex = 0; rowIndex < mapData.rows; rowIndex++) {
        const row = [];
        for (let colIndex = 0; colIndex < mapData.cols; colIndex++) {
            let newRuneType;
            do {
                newRuneType = getRandomInt(0, mapData.numTypes - 1);
            } while (
                (colIndex - 2 >= 0 &&
                    newRuneType === Number(row[colIndex - 1][1]) &&
                    newRuneType === Number(row[colIndex - 2][1])) ||
                (rowIndex - 2 >= 0 &&
                    newRuneType === Number(map[rowIndex - 1][colIndex][1]) &&
                    newRuneType === Number(map[rowIndex - 2][colIndex][1]))
            );

            let isProtected = getRandomInt(0, 100);
            if (isProtected > rate_protected) {
                isProtected = 0;
            } else {
                isProtected = 1;
            }

            row.push(String(isProtected) + String(newRuneType));
        }

        map.push(row);
    }
    console.log(map);
    return map;
}

export function generateNewRune(currentMap) {
    const newRuneMap = Array.from({ length: MapData.rows }, () =>
        Array(MapData.cols).fill("0-1")
    );

    for (let x = 0; x < MapData.cols; x++) {
        let count = 0;
        for (let y = 0; y < MapData.rows; y++) {
            if (currentMap[y][x] != -1) {
                count += 1;
            }
        }
        let isProtected = getRandomInt(0, 100);
        if (isProtected > rate_protected) {
            isProtected = 0;
        } else {
            isProtected = 1;
        }

        for (let y = 0; y < MapData.rows - count; y++) {
            newRuneMap[y][x] =
                String(isProtected) +
                String(getRandomInt(0, MapData.numTypes - 1));
        }
    }
    console.log(newRuneMap);

    return newRuneMap;
}
