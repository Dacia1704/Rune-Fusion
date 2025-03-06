import getRandomInt from "../utils/random.js";

export default function generateRuneMap(mapData) {
  const map = Array.from({ length: mapData.rows }, (_, rowIndex) => {
    const row = [];
    for (let colIndex = 0; colIndex < mapData.cols; colIndex++) {
      row.push(getRandomInt(0, mapData.numTypes - 1));
    }

    return row;
  });

  return map;
}
