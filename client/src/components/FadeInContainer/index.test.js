import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import FadeInContainer from './index';

it('renders correctly', () => {
    const component = shallow(
        <FadeInContainer>
            <div>Container content</div>
        </FadeInContainer>
    );

    expect(component).toMatchSnapshot();
});
