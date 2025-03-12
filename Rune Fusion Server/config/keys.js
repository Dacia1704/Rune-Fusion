import prodConfig from "./prod.js";
import devConfig from "./dev.js";

let config;

if (process.env.NODE_ENV === "production") {
  config = prodConfig;
} else {
  config = devConfig;
}

export default config;
