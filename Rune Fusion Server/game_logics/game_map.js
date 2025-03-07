import getRandomInt from "../utils/random.js";

export default function generateRuneMap(mapData) {
  const map = [];
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
  const rows = currentMap.Count;
  const cols = currentMap[0].Count;
  const newRuneMap = Array.from({ length: mapData.rows }, () =>
    Array(mapData.cols).fill(0)
  );
}
