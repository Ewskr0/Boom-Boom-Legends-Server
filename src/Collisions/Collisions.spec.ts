import 'mocha'
import {expect} from "chai"
import {CollisionsTiles} from "./CollisionsTiles";

describe('CollisionsTiles', () => {
    before(function() {

    });

    it('Must not collide with EMPTY_TILE', () => {
        expect(CollisionsTiles.checkEmptyCollision({x: 0, y: 0})).to.be.eq(false);
    });
    it('Must collide with FULL_TILE', () => {
        expect(CollisionsTiles.checkFullCollision({x: 0, y: 0})).to.be.eq(true);
    });
});