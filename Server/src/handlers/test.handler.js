export const testHandler = (uuid, payload) => {
    console.log(payload);
    return { status: 'success', packetId: 0 };
}

export const ChatHandler = (uuid, payload) =>{
    console.log('ChatHandler');
    console.log(payload);
    return { status: 'success', packetId: 33, payload: payload.message, broadcast: true };
}