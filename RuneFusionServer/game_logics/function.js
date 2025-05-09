export default function Clamp(value, min, max) {
    return Math.min(Math.max(value, min), max);
}
