import getRandomInt from "../utils/random.js";

let MapData;

export default function generateRuneMap(mapData) {
  const map = [];
  MapData = mapData;
  for (let rowIndex = 0; rowIndex < mapData.rows; rowIndex++) {
    const row = [];
    for (let colIndex = 0; colIndex < mapData.cols; colIndex++) {
      let newTile;
      do {
        newTile = getRandomInt(0, mapData.numTypes - 1);
      } while (
        (colIndex - 2 >= 0 &&
          newTile === row[colIndex - 1] &&
          newTile === row[colIndex - 2]) ||
        (rowIndex - 2 >= 0 &&
          newTile === map[rowIndex - 1][colIndex] &&
          newTile === map[rowIndex - 2][colIndex])
      );
      row.push(newTile);
    }

    map.push(row);
  }

  return map;
}

export function generateNewRune(currentMap) {
  const newRuneMap = Array.from({ length: MapData.rows }, () =>
    Array(MapData.cols).fill(-1)
  );

  for (let x = 0; x < MapData.cols; x++) {
    let count = 0;
    for (let y = 0; y < MapData.rows; y++) {
      if (currentMap[y][x] != -1) {
        count += 1;
      }
    }

    for (let y = 0; y < MapData.rows - count; y++) {
      newRuneMap[y][x] = getRandomInt(0, MapData.numTypes - 1);
    }
  }

  return newRuneMap;
}
