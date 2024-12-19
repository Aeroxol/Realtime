import { reqGameAssets } from "../init/assets.js";
import { gameEnd, gameStart, getItem } from "./game.handler.js";
import { moveStageHandler } from "./stage.handler.js";
import { testHandler } from "./test.handler.js";

const handlerMappings = {
    0: testHandler,
    1: reqGameAssets,
    2: gameStart,
    3: gameEnd,
    11: moveStageHandler,
    22: getItem,
    //33: sendMessage,
};

export default handlerMappings;