/// user { uuid, socketId }
const users = [];

/// user { uuid, socketId }
export const addUser = (user) => {
    users.push(user);
};

export const getUser = () => {
    return users;
}

/// socketId
export const removeUser = (socketId) => {
    const index = users.findIndex((user) => user.socketId === socketId);
    if (index !== -1) {
        return users.splice(index, 1)[0];
    }
}