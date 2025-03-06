import { MapDataPayload } from "../models/mapData";
import getRandomInt from "../utils/random";

export function generateRuneMap(mapData: MapDataPayload) {
  const map = Array.from({ length: mapData.rows }, () =>
    Array(mapData.cols).fill(getRandomInt(0, mapData.numTypes - 1))
  );
  return map;
}
