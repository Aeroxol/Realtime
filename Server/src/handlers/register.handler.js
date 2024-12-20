import { addUser } from "../models/user.model.js";
import { v4 as uuidv4 } from 'uuid';
import { handleConnection, handleDisconnect, handleEvent } from './helper.js';

const registerHandler = (io) => {
    io.on('connection', (socket) => {
        // 최초 커넥션을 맺은 이후 발생하는 각종 이벤트를 처리하는 곳
        const userUUID = uuidv4();
        console.log(userUUID);
        
        addUser({ uuid: userUUID, socketId: socket.id });

        handleConnection(socket, userUUID);

        socket.on('event', (data) => handleEvent(io, socket, data));
        socket.on('disconnect', () => {
            handleDisconnect(socket, userUUID)
        });
    });
};

export default registerHandler;