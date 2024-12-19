import { CLIENT_VERSION } from "../constatns.js";
import { getGameAssets } from "../init/assets.js";
import { createStage } from "../models/stage.model.js";
import { getUser, removeUser } from "../models/user.model.js"
import handlerMappings from "./handlerMapping.js";

export const handleDisconnect = (socket, uuid) => {
    removeUser(socket.id);
    console.log(`User disconnected : ${uuid}`);
    console.log(`Current users: `, getUser());
}

export const handleConnection = (socket, uuid) => {
    console.log(`new User connected: ${uuid} with socket ID ${socket.id}`);
    console.log(`Current users: `, getUser());

    createStage(uuid);

    socket.emit('connection', uuid);
}

// data = { clientVersion, handlerId, userId, payload }
export const handleEvent = (io, socket, data) => {
    if (!CLIENT_VERSION.includes(data.clientVersion)) {
        socket.emit('response', { status: 'fail', message: 'Client version X' });
        return;
    }

    const handler = handlerMappings[data.handlerId];
    if (!handler) {
        socket.emit('response', { status: 'fail', message: 'handler not found' });
        return;
    }

    const response = handler(data.userId, data.payload);

    if (response.broadcast) {
        io.emit('response', 'broadcast');
        return;
    }

    socket.emit('response', response);
}