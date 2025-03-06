/**
 * Get Random Integer
 * @param {int} min
 * @param {int} max
 * @returns [min,max]
 */
function getRandomInt(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

// export { getRandomInt };
export default getRandomInt;
